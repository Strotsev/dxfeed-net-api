﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using com.dxfeed.api;
using com.dxfeed.api.events;
using com.dxfeed.native.api;
using com.dxfeed.native.events;

namespace com.dxfeed.native {
	public class NativeSubscription : IDxSubscription {
		private readonly IntPtr connectionPtr;
		private IntPtr subscriptionPtr;
		private readonly IDxFeedListener listener;
		//to prevent callback from being garbage collected
		private readonly C.dxf_event_listener_t callback;

		public NativeSubscription(NativeConnection connection, EventType eventType, IDxFeedListener listener) {
			if (listener == null)
				throw new ArgumentNullException("listener");

			connectionPtr = connection.Handler;
			this.listener = listener;

			C.CheckOk(C.Instance.dxf_create_subscription(connectionPtr, eventType, out subscriptionPtr));
			try {
				C.CheckOk(C.Instance.dxf_attach_event_listener(subscriptionPtr, callback = OnEvent, IntPtr.Zero));
			} catch (DxException) {
				C.Instance.dxf_close_subscription(subscriptionPtr);
				throw;
			}
		}

		private void OnEvent(EventType eventType, IntPtr symbol, IntPtr data, int dataCount, IntPtr userData) {
			switch (eventType) {
				case EventType.Order:
					var orderBuf = NativeBufferFactory.CreateOrderBuf(symbol, data, dataCount);
					listener.OnOrder<NativeEventBuffer<NativeOrder>, NativeOrder>(orderBuf);
					break;
				case EventType.Profile:
					var profileBuf = NativeBufferFactory.CreateProfileBuf(symbol, data, dataCount);
					listener.OnProfile<NativeEventBuffer<NativeProfile>, NativeProfile>(profileBuf);
					break;
				case EventType.Quote:
					var quoteBuf = NativeBufferFactory.CreateQuoteBuf(symbol, data, dataCount);
					listener.OnQuote<NativeEventBuffer<NativeQuote>, NativeQuote>(quoteBuf);
					break;
				case EventType.TimeAndSale:
					var tsBuf = NativeBufferFactory.CreateTimeAndSaleBuf(symbol, data, dataCount);
					listener.OnTimeAndSale<NativeEventBuffer<NativeTimeAndSale>, NativeTimeAndSale>(tsBuf);
					break;
                case EventType.Trade:
                    var tBuf = NativeBufferFactory.CreateTradeBuf(symbol, data, dataCount);
                    listener.OnTrade<NativeEventBuffer<NativeTrade>, NativeTrade>(tBuf);
                    break;
                case EventType.Summary:
                    var sBuf = NativeBufferFactory.CreateSummaryBuf(symbol, data, dataCount);
                    listener.OnFundamental<NativeEventBuffer<NativeSummary>, NativeSummary>(sBuf);
                    break;
			}
		}

		public void AddSymbol(string symbol) {
			C.CheckOk(C.Instance.dxf_add_symbol(subscriptionPtr, symbol));
		}

		#region Implementation of IDisposable

		public void Dispose() {
			if (subscriptionPtr == IntPtr.Zero) return;
			
			C.CheckOk(C.Instance.dxf_close_subscription(subscriptionPtr));
			subscriptionPtr = IntPtr.Zero;
		}

		#endregion

		#region Implementation of IDxSubscription

		public void AddSymbols(params string[] symbols) {
			C.CheckOk(C.Instance.dxf_add_symbols(subscriptionPtr, symbols, symbols.Length));
		}

		public void RemoveSymbols(params string[] symbols) {
			C.CheckOk(C.Instance.dxf_remove_symbols(subscriptionPtr, symbols, symbols.Length));
		}

		public void SetSymbols(params string[] symbols) {
			C.CheckOk(C.Instance.dxf_set_symbols(subscriptionPtr, symbols, symbols.Length));
		}

		public void Clear() {
			C.CheckOk(C.Instance.dxf_clear_symbols(subscriptionPtr));
		}

		public unsafe IList<string> GetSymbols() {
			IntPtr head;
			int len;
			C.CheckOk(C.Instance.dxf_get_symbols(subscriptionPtr, out head, out len));
			
			var result = new string[len];
			for(var i = 0; i < len; i++) {
				var ptr = *(IntPtr*)IntPtr.Add(head, IntPtr.Size*i);
				result[i] = Marshal.PtrToStringUni(ptr);
			}

			return result;
		}

		public void AddSource(params string[] sources) {
			Encoding ascii = Encoding.ASCII;
			for (int i = 0; i < sources.Length; i++) {
				byte[] source = ascii.GetBytes(sources[i]);
				C.CheckOk(C.Instance.dxf_add_order_source(subscriptionPtr, source));
			}
		}

		public void SetSource(params string[] sources) {
			if (sources.Length == 0)
				return;
			Encoding ascii = Encoding.ASCII;
			byte[] source = ascii.GetBytes(sources[0]);
			C.CheckOk(C.Instance.dxf_set_order_source(subscriptionPtr, source));
			for (int i = 1; i < sources.Length; i++) {
				source = ascii.GetBytes(sources[i]);
				C.CheckOk(C.Instance.dxf_add_order_source(subscriptionPtr, source));
			}
		}

		#endregion
	}
}