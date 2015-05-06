﻿using PagedList.Mvc;

namespace Workshop.ViewModels
{
    public class PagingOptions
    {
        public static PagedListRenderOptions Standard
        {
            get
            {
                var options = new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    LinkToPreviousPageFormat = "上一頁",
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    LinkToNextPageFormat = "下一頁",
                    DisplayLinkToLastPage = PagedListDisplayMode.Always
                };

                return options;
            }
        }
    }
}