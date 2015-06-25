﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grabacr07.KanColleViewer.Models;
using Livet.Messaging.Windows;

namespace Grabacr07.KanColleViewer.ViewModels
{
	public class ProxyBootstrapperViewModel : WindowViewModel
	{
		#region messages (const)
		// いつか多言語リソースに移す… いつか…
		private const string message10048 = @"既にポート {0} で通信を待ち受けているアプリケーションが存在するため、開始に失敗しました。
アプリケーションを終了するか、以下で待ち受けポートを変更できます。";
		private const string messageUnexpectedException = @"だめだった :;(∩´﹏`∩);:
{0}";
		#endregion

		public bool DialogResult { get; private set; }

		public ProxyBootstrapper Bootstrapper { get; private set; }

		#region Message 変更通知プロパティ

		private string _Message;

		public string Message
		{
			get { return this._Message; }
			set
			{
				if (this._Message != value)
				{
					this._Message = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion


		public ProxyBootstrapperViewModel(ProxyBootstrapper bootstrapper)
		{
			this.DialogResult = false;
			this.Bootstrapper = bootstrapper;
			this.UpdateMessage();
		}

		public void Retry()
		{
			this.Bootstrapper.Try();

			if (this.Bootstrapper.Result == ProxyBootstrapResult.Success)
			{
				this.DialogResult = true;
				this.Close();
			}
			else
			{
				this.UpdateMessage();
			}
		}

		public void Cancel()
		{
			this.DialogResult = false;
			this.Close();
		}

		public void Close()
		{
			this.Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Window/Close"));
		}

		private void UpdateMessage()
		{
			if (this.Bootstrapper.Result == ProxyBootstrapResult.WsaEAddrInUse)
			{
				this.Message = string.Format(message10048, this.Bootstrapper.ListeningPort);
			}
			else if (this.Bootstrapper.Result == ProxyBootstrapResult.UnexpectedException)
			{
				this.Message = string.Format(messageUnexpectedException, this.Bootstrapper.Exception.Message);
			}
		}
	}
}