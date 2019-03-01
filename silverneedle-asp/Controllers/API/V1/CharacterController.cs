using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using SilverNeedle;
using SilverNeedle.Actions.CharacterGeneration;
using SilverNeedle.Characters;
using SilverNeedle.Dice;
using SilverNeedle.Serialization;

namespace silverneedleweb.Controllers.API.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CharacterAPIController : Controller
    {
        private EntityGateway<CharacterStrategy> strategyGateway = GatewayProvider.Get<CharacterStrategy>();

        [HttpGet]
        public ActionResult<CharacterSheetTextView> Index(string strategy, int level, string scores)
        {
            
            var build = strategyGateway.Find(strategy);
            var roller = GatewayProvider.Find<AbilityScoreGenerator>(scores);
            build.TargetLevel = level;
            build.AbilityScoreRoller = roller.Generator;

            var gen = GatewayProvider.Find<CharacterDesigner>(build.Designer);
            
            var character = new CharacterSheet(build);
            gen.ExecuteStep(character);
            
//           ViewData["character"] = new CharacterSheetTextView(character);
//            ViewData["characterFull"] = character;
//            ViewData["strategy"] = strategy;
//            ViewData["scores"] = scores;
//            ViewData["level"] = level;

//            var saveObj = new YamlObjectStore();
//            character.Save(saveObj);
            //ViewData["save-data"] = saveObj.WriteToString();
            return new CharacterSheetTextView(character);

        }
    }

   


    
    

   
}