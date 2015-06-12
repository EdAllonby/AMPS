using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
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

        public BurnDownViewModel(Band band, IServiceRegistry serviceRegistry) : base(serviceRegistry)
        {
            this.band = band;
            taskRepository = (TaskRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Task>();
            jamRepository = (JamRepository) serviceRegistry.GetService<RepositoryManager>().GetRepository<Jam>();

            Jam jam = jamRepository.GetCurrentActiveJamInBand(band.Id);

            int totalPoints = taskRepository.GetTasksInJam(jam.Id).Sum(x => x.Points);
            int startingPosition = 0;
            Points = new List<DataPoint>();

            Points.Add(new DataPoint(startingPosition, totalPoints));

            foreach (Task task in taskRepository.GetTasksInJam(jam.Id))
            {
                startingPosition += 10;
                totalPoints -= task.Points;

                Points.Add(new DataPoint(startingPosition, totalPoints));
            }
        }

        public IList<DataPoint> Points { get; private set; }
    }
}