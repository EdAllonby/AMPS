using System.Collections.Generic;
using System.Linq;
using OxyPlot;
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
        private IList<DataPoint> points;

        public BurnDownViewModel(Band band, IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
            this.band = band;
            taskRepository = (TaskRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();
            jamRepository = (JamRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

            taskRepository.EntityAdded += JamUpdated;
            taskRepository.EntityUpdated += JamUpdated;

            UpdatePoints();
        }

        public IList<DataPoint> Points
        {
            get { return points; }
            set
            {
                points = value;
                OnPropertyChanged();
            }
        }

        private void UpdatePoints()
        {
            Jam jam = jamRepository.GetCurrentActiveJamInBand(band.Id);

            if (jam != null)
            {
                int totalPoints = taskRepository.GetTasksInJam(jam.Id).Sum(x => x.Points);
                int startingPosition = 0;

                Points = new List<DataPoint>();

                Points.Add(new DataPoint(startingPosition, totalPoints));

                foreach (Task task in taskRepository.GetTasksInJam(jam.Id).ToList())
                {
                    startingPosition += 10;
                    totalPoints -= task.Points;

                    Points.Add(new DataPoint(startingPosition, totalPoints));
                }
            }
        }

        private void JamUpdated(object sender, EntityChangedEventArgs<Task> e)
        {
            UpdatePoints();
        }
    }
}