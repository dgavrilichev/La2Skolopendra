using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using La2Skolopendra.Export;

namespace La2Skolopendra.Engine
{
    internal class MasterProcessor
    {
        private readonly IntPtr _hWnd;
        [NotNull] private readonly SkSettings _settings;

        internal MasterProcessor(IntPtr hWnd, [NotNull] SkSettings settings)
        {
            _hWnd = hWnd;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        internal async Task Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var point = await GetTargetPoint(cancellationToken);


                await Task.Delay(TimeSpan.FromSeconds(0.1), cancellationToken);
            }
        }

        private async Task<Point?> GetTargetPoint(CancellationToken cancellationToken)
        {



            throw new NotImplementedException();
        }
    }
}
