﻿#region License

/*
Copyright (c) 2010-2021 Devexperts LLC

This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion

using System;
using com.dxfeed.api.events;

namespace com.dxfeed.api
{
    public interface IDxRegionalBookListener
    {
        void OnChanged(DxPriceLevelBook book);
    }
}
