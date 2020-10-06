namespace Graphyte.Build
{
    public abstract class Project
    {
        private string m_Name;
        public string Name
        {
            get
            {
                if (this.m_Name == null)
                {
                    return this.GetType().Name;
                }
                return this.m_Name;
            }

            protected set => this.m_Name = value;
        }

        protected string ProjectFileName { get; set; }

        public abstract void Configure(Target target);
    }
}
