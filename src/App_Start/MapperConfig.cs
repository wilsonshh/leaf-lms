using AutoMapper;
using System.Web;
using WebApplication.Extensions;
using WebApplication.Models;

namespace WebApplication.App_Start
{
    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize
                (x =>
            {
                #region Annoucement

                x.CreateMap<Announcement, AnnouncementViewModel>();
                x.CreateMap<AnnouncementViewModel, Announcement>();

                #endregion Annoucement

                #region AnnouncementRead

                x.CreateMap<AnnouncementRead, AnnouncementReadViewModel>()
                    .AfterMap((src, dest) => dest.Status = "seen");

                #endregion AnnouncementRead

                #region Comment

                x.CreateMap<CommentReadOnlyViewModel, Comment>()
                    .AfterMap(((src, dest) => dest.Text = HttpUtility.HtmlEncode(src.Text.ExtractHtmlInnerText())));

                x.CreateMap<Comment, CommentReadOnlyViewModel>()
                    .AfterMap(((src, dest) => dest.Text = HttpUtility.HtmlDecode(src.Text)));

                x.CreateMap<CommentViewModel, Comment>()
                    .AfterMap(((src, dest) => dest.Text = HttpUtility.HtmlEncode(src.Text.ExtractHtmlInnerText())));

                x.CreateMap<Comment, CommentViewModel>()
                    .AfterMap(((src, dest) => dest.Text = HttpUtility.HtmlDecode(src.Text)));

                #endregion Comment
            });
        }
    }
}