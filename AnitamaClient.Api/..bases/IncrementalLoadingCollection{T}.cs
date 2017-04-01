using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using static System.Runtime.InteropServices.WindowsRuntime.AsyncInfo;

namespace AnitamaClient
{
    public abstract class IncrementalLoadingCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        protected IncrementalLoadingCollection()
        {
        }

        private int recordCount = -1, pageCount = 1;

        public int RecordCount
        {
            get => this.recordCount;
            protected set => Set(nameof(IsEmpty), ref this.recordCount, value);
        }

        public int PageCount
        {
            get => this.pageCount;
            protected set => Set(nameof(HasMoreItems), nameof(LoadedPageCount), ref this.pageCount, value);
        }

        protected abstract IAsyncOperation<IEnumerable<T>> LoadPageAsync(int pageIndex);

        public bool IsEmpty => this.RecordCount == 0;

        private int loadedPageCount;

        protected int LoadedPageCount => this.loadedPageCount;

        public bool HasMoreItems => this.loadedPageCount < this.PageCount;

        protected void ResetAll()
        {
            this.PageCount = 1;
            this.RecordCount = -1;
            this.loadedPageCount = 0;
            Clear();
        }

        private IAsyncOperation<LoadMoreItemsResult> loading;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if(this.loading?.Status == AsyncStatus.Started)
            {
                var temp = this.loading;
                return Run(async token =>
                {
                    token.Register(temp.Cancel);
                    while(temp.Status == AsyncStatus.Started)
                    {
                        await Task.Delay(200);
                    }
                    switch(temp.Status)
                    {
                    case AsyncStatus.Completed:
                        return temp.GetResults();
                    case AsyncStatus.Error:
                        throw temp.ErrorCode;
                    default:
                        token.ThrowIfCancellationRequested();
                        throw new OperationCanceledException(token);
                    }
                });
            }
            return this.loading = Run(async token =>
            {
                if(!this.HasMoreItems)
                    return new LoadMoreItemsResult();
                var lp = LoadPageAsync(this.loadedPageCount);
                token.Register(lp.Cancel);
                count = 0;
                try
                {
                    var re = await lp;
                    count = (uint)this.AddRange(re);
                    this.loadedPageCount++;
                    RaisePropertyChanged(nameof(HasMoreItems));
                }
                catch(Exception ex)
                {
                    if(!await tryHandle(ex))
                        throw;
                }
                return new LoadMoreItemsResult() { Count = count };
            });
        }

        public event TypedEventHandler<IncrementalLoadingCollection<T>, LoadMoreItemsExceptionEventArgs> LoadMoreItemsException;

        private async Task<bool> tryHandle(Exception ex)
        {
            var temp = LoadMoreItemsException;
            if(temp == null)
                return false;
            var h = false;
            await DispatcherHelper.RunAsync(() =>
            {
                var args = new LoadMoreItemsExceptionEventArgs(ex);
                temp(this, args);
                h = args.Handled;
            });
            return h;
        }
    }

    public class LoadMoreItemsExceptionEventArgs : EventArgs
    {
        internal LoadMoreItemsExceptionEventArgs(Exception ex)
        {
            this.Exception = ex;
        }

        public Exception Exception
        {
            get;
        }

        public string Message => this.Exception?.Message;

        public bool Handled
        {
            get;
            set;
        }
    }
}
