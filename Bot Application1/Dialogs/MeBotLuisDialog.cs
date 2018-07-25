using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis.Models;
using System.Configuration;


namespace Bot_Application1.Dialogs
{
    
    [Serializable]
    public class MeBotLuisDialog: LuisDialog<object>
    {
        public MeBotLuisDialog() : base(
            new LuisService(
                new LuisModelAttribute(
                    ConfigurationManager.AppSettings["LuisAppId"],
                    ConfigurationManager.AppSettings["LuisAPIKey"],
                    domain: "westus.api.cognitive.microsoft.com"

                    )))
        { }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("This is about people in our group.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("AboutMe")]
        public async Task AboutMe(IDialogContext context, LuisResult result)
        {
            EntityRecommendation personName;
            result.TryFindEntity("persons name", out personName);
            if (personName != null && personName.Entity != null && !string.IsNullOrWhiteSpace(personName.Entity))
            {
                if (personName.Entity.ToLower().Contains("jeff"))
                    await context.PostAsync(@"Jeff is a Software Engineer currently working at UPMC SouthSide Hospital. He started his professional career in 1975  after completing his graduation as MS in Computer Science.");

                else if (personName.Entity.ToLower().Contains("floyd"))
                    await context.PostAsync(@"Floyd is a Software Engineer currently working at UPMC SouthSide Hospital.");
                else
                    await context.PostAsync($"I do not know {personName.Entity}.");

            }
            else
                await context.PostAsync(@"Please supply a name");


            context.Wait(MessageReceived);
        }


    }
}