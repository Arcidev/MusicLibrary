﻿using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;

namespace Tests
{
    public class SingletonEqualizer : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            model.LifestyleType = LifestyleType.Singleton;
        }
    }
}
