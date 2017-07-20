using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Common.Utility
{
    public class Aspect
    {
        public Action<Action> Chain = null;
        public Delegate WorkDelegate;
        public static Aspect Define
        {
            get
            {
                return new Aspect();
            }
        }
        public Aspect()
        {

        }

        public Aspect Combine(Action<Action> action)
        {
            Action<Action> variable = null;
            if (this.Chain != null)
            {
                Action<Action> chain = this.Chain;
                this.Chain = (Action work) =>
                {
                    Action<Action> v = variable;
                    chain(new Action(() => v(work)));
                };
            }
            else
            {
                this.Chain = action;
            }
            return this;
        }

        public void DoWork(Action work)
        {
            if (this.Chain != null)
            {
                this.Chain(work);
            }
            else
            {
                work();
            }
        }

        public TReturnType Return<TReturnType>(Func<TReturnType> work)
        {
            TReturnType tReturnType;
            this.WorkDelegate = work;
            if (this.Chain != null)
            {
                TReturnType workDelegate = default(TReturnType);
                this.Chain(new Action(() => { workDelegate = (this.WorkDelegate as Func<TReturnType>)(); }));
                tReturnType = workDelegate;
            }
            else
            {
                tReturnType = work();
            }
            return tReturnType;
        }
    }
}
