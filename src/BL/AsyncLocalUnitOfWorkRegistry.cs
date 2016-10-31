using Riganti.Utils.Infrastructure.Core;
using System.Threading;
using System.Collections.Generic;

namespace BL
{
    public class AsyncLocalUnitOfWorkRegistry : UnitOfWorkRegistryBase
    {
        private readonly AsyncLocal<Stack<IUnitOfWork>> stack = new AsyncLocal<Stack<IUnitOfWork>>();

        public AsyncLocalUnitOfWorkRegistry()
        {
            stack.Value = new Stack<IUnitOfWork>();
        }

        protected override Stack<IUnitOfWork> GetStack()
        {
            return stack.Value;
        }
    }
}
