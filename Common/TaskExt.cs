using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class TaskExt
    {
        public static bool RunWait(Action action, int millSecondsTimeout, bool isThrowExp = false)
        {
            Thread thread = null;

            var task = Task.Run(() =>
            {
                thread = Thread.CurrentThread;
                action();
            });

            task.Wait(millSecondsTimeout);

            if (task.Status != TaskStatus.RanToCompletion)
            {
                thread.Abort();

                if (isThrowExp)
                {
                    throw new TimeoutException("任务执行超时！");
                }

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
