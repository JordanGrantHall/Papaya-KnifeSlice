using System.Collections;

namespace KnifeSlicer.Tasks
{
    public interface ITask
    {
        public IEnumerator ExecuteInternal();
        public bool CanExecute();
        public bool IsFinished();
    }
}