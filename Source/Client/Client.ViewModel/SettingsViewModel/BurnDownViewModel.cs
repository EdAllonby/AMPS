using System;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Shared;
using Shared.Domain;
using Shared.Repository;

namespace Client.ViewModel.SettingsViewModel
{
    public class BurnDownViewModel : ViewModel
    {
        private readonly Band band;
        private readonly JamRepository jamRepository;
        private readonly TaskRepository taskRepository;
        private PlotModel plotModel;

        public BurnDownViewModel(Band band, IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
            this.band = band;
            taskRepository = (TaskRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();
            jamRepository = (JamRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

            taskRepository.EntityAdded += JamUpdated;
            taskRepository.EntityUpdated += JamUpdated;

            PlotModel = new PlotModel {Title = "Jam Burndown"};

            UpdateModel();
        }

        public PlotModel PlotModel
        {
            get { return plotModel; }
            set
            {
                plotModel = value;
                OnPropertyChanged();
            }
        }

        private Series CreateActualBurndown()
        {
            Jam jam = jamRepository.GetCurrentActiveJamInBand(band.Id);

            var actualBurndown = new LineSeries
            {
                Title = "Actual Burndown",
                StrokeThickness = 2,
                MarkerSize = 1,
                MarkerStroke = OxyColors.DarkRed,
                MarkerType = MarkerType.Diamond,
                CanTrackerInterpolatePoints = false
            };

            if (jam != null)
            {
                int totalPoints = taskRepository.GetTasksInJam(jam.Id).Sum(x => x.Points);

                int days = 0;

                actualBurndown.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(days)), totalPoints));

                foreach (Task task in taskRepository.GetTasksInJam(jam.Id).ToList())
                {
                    totalPoints -= task.Points;
                    days++;

                    actualBurndown.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(days)), totalPoints));
                }
            }

            return actualBurndown;
        }

        private Series CreateExpectedBurndown()
        {
            var projectedBurndownSeries = new LineSeries
            {
                Title = "Projected Burndown",
                StrokeThickness = 2,
                MarkerSize = 1,
                MarkerStroke = OxyColors.Coral,
                MarkerType = MarkerType.Star,
                CanTrackerInterpolatePoints = false
            };

            Jam jam = jamRepository.GetCurrentActiveJamInBand(band.Id);

            if (jam != null)
            {
                int totalPoints = taskRepository.GetTasksInJam(jam.Id).Sum(x => x.Points);

                var daysAhead = 0;

                projectedBurndownSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddDays(daysAhead)), totalPoints));

                const int projectedPointsPerDay = 7;

                while (totalPoints > 0)
                {
                    daysAhead++;

                    DateTime nextDate = DateTime.Now.AddDays(daysAhead);

                    if (IsWorkingDay(nextDate))
                    {
                        if (totalPoints <= projectedPointsPerDay)
                        {
                            totalPoints = 0;
                        }
                        else
                        {
                            totalPoints -= projectedPointsPerDay;
                        }
                    }

                    projectedBurndownSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(nextDate), totalPoints));
                }
            }

            return projectedBurndownSeries;
        }

        private static bool IsWorkingDay(DateTime nextDate)
        {
            return nextDate.DayOfWeek != DayOfWeek.Saturday && nextDate.DayOfWeek != DayOfWeek.Sunday;
        }

        private void UpdateModel()
        {
            PlotModel = new PlotModel();

            var dateAxis = new DateTimeAxis {Title = "Projected Finish"};
            PlotModel.Axes.Add(dateAxis);
            var valueAxis = new LinearAxis {Title = "Task Points Remaining"};
            PlotModel.Axes.Add(valueAxis);

            PlotModel.Series.Add(CreateExpectedBurndown());
            PlotModel.Series.Add(CreateActualBurndown());
        }

        private void JamUpdated(object sender, EntityChangedEventArgs<Task> e)
        {
            UpdateModel();
        }
    }
}