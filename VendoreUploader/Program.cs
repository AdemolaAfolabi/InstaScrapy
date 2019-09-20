using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Dropbox.Api;
using System.IO;
using System.Text;
using Dropbox.Api.Files;

namespace VendoreUploader
{
    class Program
    {
        public static InstaApiBuilder InstagramClient { get; set; }

        public static DropboxClient DbxClient { get; set; }

        static void Main(string[] args)
        {
            var result = Task
                .Run(MainAsync)
                .GetAwaiter()
                .GetResult();
            if (result)
                return;

            Console.ReadKey();
        }

        public static async Task<bool> MainAsync()
        {
            await GetMediaByUser("champagnepapi");
            return true;
        }

        public static async Task UploadMedia(IResult<InstaMediaList> Media, string User)
        {
            var UriClient = new HttpClient();
            DbxClient = new DropboxClient("fwtH1CHrKCAAAAAAAAAN5ov7bATlPsZrBXWA1mdD7WcwbfmqEknoUQgsH60s5Vt9");
            if (!string.IsNullOrEmpty(User))
            {
                foreach (var MediaItem in Media.Value)
                {
                    if (MediaItem.Videos.Count > 0)
                    {
                        foreach (var Video in MediaItem.Videos)
                        {
                            var Response = await UriClient.GetStreamAsync(Video.Url);
                            using (Response)
                            {
                                var Updated = await DbxClient.Files.UploadAsync("/" + User + "/" + MediaItem.InstaIdentifier + ".mp4", WriteMode.Overwrite.Instance, body: Response);
                            }
                        }
                    }

                    if (MediaItem.Images.Count > 0)
                    {
                        foreach (var Image in MediaItem.Images)
                        {
                            var Response = await UriClient.GetStreamAsync(Image.URI);
                            using (Response)
                            {
                                var Updated = await DbxClient.Files.UploadAsync("/"+User+"/"+MediaItem.InstaIdentifier+".jpg", WriteMode.Overwrite.Instance, body: Response);
                            }
                        }
                    }
                }
            }
        }

        public static async Task<IResult<InstaMediaList>> GetMediaByUser(string User)
        {
            try
            {
                InstagramClient = (InstaApiBuilder)InstaApiBuilder.CreateBuilder();
                var Credentials = new UserSessionData();
                Credentials.UserName = "demzzy_";
                Credentials.Password = "afolabi1994";
                var AuthenicatedUser = Credentials.WithPassword("afolabi1994");


                var Api = InstagramClient
                    .SetUser(AuthenicatedUser)
                    .Build();

                var Result = await Api.LoginAsync();
                Console.WriteLine(Result.Succeeded);

                var Parameters = PaginationParameters.MaxPagesToLoad(10);
                IResult<InstaMediaList> Media = await Api.GetUserMediaAsync("demzzy_", Parameters);
                await UploadMedia(Media, User);
                return Media;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
