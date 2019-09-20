
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

namespace InstagramScraperCLI.Service {
       public class UserService {

        public UserService(string AccessToken) {
            this.DbxClient = new DropboxClient(AccessToken);
            this.InstagramClient = (InstaApiBuilder)InstaApiBuilder.CreateBuilder();
        }

        public InstaApiBuilder InstagramClient { get; set; }

        public DropboxClient DbxClient { get; set; }

        public async Task UploadMedia(IResult<InstaMediaList> Media, string User, string AccessToken)
        {
            try
            {
                var UriClient = new HttpClient();
                int ImageIterator = 0;
                if (!string.IsNullOrEmpty(User))
                {
                    foreach (var MediaItem in Media.Value)
                    {
                        if (MediaItem.Images.Count > 0)
                        {
                            foreach (var Image in MediaItem.Images)
                            {
                                var Response = await UriClient.GetStreamAsync(Image.URI);
                                using (Response)
                                {
                                    var Updated = await DbxClient.Files.UploadAsync($"/{User}/{MediaItem.InstaIdentifier}.jpg", WriteMode.Overwrite.Instance, body: Response);
                                    ImageIterator++;
                                    Console.WriteLine($"{Updated.Name} image has been uploaded for {User}");
                                }
                            }
                        }
                    }

                    Console.WriteLine($"{ImageIterator} have been uploaded for ${User} listing images");
                }
            }

            catch (RetryException e)
            {
                Console.WriteLine(e.Message);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public async Task<IResult<InstaMediaList>> GetMediaByUser(string User)
        {
            try
            {
                var Credentials = new UserSessionData();
                Credentials.UserName = "demzzy_";
                Credentials.Password = "afolabi1994";
                string AccessToken = "fwtH1CHrKCAAAAAAAAAN5ov7bATlPsZrBXWA1mdD7WcwbfmqEknoUQgsH60s5Vt9";

                var Api = InstagramClient
                    .SetUser(Credentials)
                    .Build();

                var Result = await Api.LoginAsync();
                Console.WriteLine(Result.Succeeded);

                var Parameters = PaginationParameters.MaxPagesToLoad(10);
                IResult<InstaMediaList> Media = await Api.GetUserMediaAsync(User, Parameters);
                await UploadMedia(Media, User, AccessToken);
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