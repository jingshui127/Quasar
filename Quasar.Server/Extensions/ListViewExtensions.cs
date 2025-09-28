﻿using Quasar.Common.Helpers;
using Quasar.Server.Helper;
using Quasar.Server.Utilities;
using System;
using System.Windows.Forms;

namespace Quasar.Server.Extensions
{
    public static class ListViewExtensions
    {
        private const uint SET_COLUMN_WIDTH = 4126;
        private static readonly IntPtr AUTOSIZE_USEHEADER = new IntPtr(-2);

        /// <summary>
        /// 自动确定给定列表视图上的正确列大小。
        /// </summary>
        /// <param name="targetListView">要自动调整列大小的列表视图。</param>
        public static void AutosizeColumns(this ListView targetListView)
        {
            if (PlatformHelper.RunningOnMono) return;
            for (int lngColumn = 0; lngColumn <= (targetListView.Columns.Count - 1); lngColumn++)
            {
                NativeMethods.SendMessage(targetListView.Handle, SET_COLUMN_WIDTH, new IntPtr(lngColumn), AUTOSIZE_USEHEADER);
            }
        }

        /// <summary>
        /// 选择给定列表视图上的所有项目。
        /// </summary>
        /// <param name="targetListView">要选择项目的列表视图。</param>
        public static void SelectAllItems(this ListView targetListView)
        {
            NativeMethodsHelper.SetItemState(targetListView.Handle, -1, 2, 2);
        }
    }
}