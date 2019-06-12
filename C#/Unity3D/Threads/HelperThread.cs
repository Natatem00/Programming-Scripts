namespace Helpers.Thread
{
    using System.Threading;
    public abstract class HelperThread
    {
        bool isDone = false;
        Thread thread = null;
		
		object locker = new object();


        /// <summary>
        /// starts the thread to processing the ThreadFunction
        /// </summary>
        public virtual void Start()
        {
            thread = new Thread(Run);
            thread.Start();
        }

        public virtual void Aboart()
        {
            if (thread != null)
            {
                thread.Abort();
            }
        }

        /// <summary>
        /// thread calling this function when finished the ThreadFunction
        /// </summary>
        public virtual void OnFinished() { }
        /// <summary>
        /// function to process
        /// </summary>
        public virtual void ThreadFunction() { }
        /// <summary>
        /// checks thread status
        /// </summary>
        public virtual bool Update()
        {
            if (isDone)
            {
                OnFinished();
                thread.Abort();
                return true;
            }
            return false;
        }
        /// <summary>
        /// thread function
        /// </summary>
        public virtual void Run()
        {
			lock(locker)
			{
            ThreadFunction();
            isDone = true;
			}
        }
    }
}