﻿#region License

/*
Copyright (c) 2010-2021 Devexperts LLC

This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion

using System;

namespace com.dxfeed.api.events
{
    public struct DxPriceLevelBook
    {
        public struct DxPriceLevel
        {
            public double Price { get; }
            public double Size { get; }
            public DateTime Time { get; }

            public DxPriceLevel(double price, double size, DateTime time)
            {
                this.Price = price;
                this.Size = size;
                this.Time = time;
            }
        }

        public string Symbol { get; }
        public DxPriceLevel[] Bids { get; }
        public DxPriceLevel[] Asks { get; }

        public DxPriceLevelBook(string symbol, DxPriceLevel[] bids, DxPriceLevel[] asks)
        {
            this.Symbol = symbol;
            this.Bids = bids;
            this.Asks = asks;
        }
    }
}
