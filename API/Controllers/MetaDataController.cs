using Microsoft.AspNetCore.Mvc;
using Iphone14.Models;
using Microsoft.AspNetCore.Authorization;
using YamlDotNet.Serialization;

namespace Iphone14.Controllers
{
    /**The MetaDataController class provides API endpoints for retrieving metadata.
                                                                    * It includes authorization and two actions:
                                                                    * - GetMenu: Retrieves menu data from a YAML file and returns it as JSON.
                                                                    * - GetLayout: Retrieves layout data based on the entity and layout type, specified in the route and query parameters respectively.
                                                                    *   The layout data is read from a YAML file and returned as JSON.
                                                                    */
    [Route("api/meta-data")]
    [Authorize]
    public class MetaDataController : ControllerBase
    {
        /// <summary>
        /// Retrieves and returns menu data
        /// </summary>
        /// <returns>Returns json.</returns>
        [HttpGet]
        [Route("menu")]
        public IActionResult GetMenu()
        {
            string menuFilePath =  $"./Menu/Menu.yaml";
            var dynamicYaml = System.IO.File.ReadAllText(menuFilePath);
            var deserializer =  new DeserializerBuilder().Build();
            var yamlObject = deserializer.Deserialize<dynamic>(dynamicYaml);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(yamlObject, Newtonsoft.Json.Formatting.Indented);
            return Ok(json);
        }

        /// <summary>
        /// Retrieves and returns layout data based on entity and layout type.
        /// </summary>
        /// <param name="entity">Entity name</param>
        /// <param name="layoutType">Layout type as List = 1, CardView = 2</param>
        /// <returns>Returns json.</returns>
        [HttpGet]
        [Route("{entity}/layout")]
        public IActionResult GetLayout([FromRoute] string entity, [FromQuery] LayoutType layoutType)
        {
            if (string.IsNullOrEmpty(entity))
            {
                return BadRequest("Entity should not be empty");
            }

            if (layoutType == 0)
            {
                return BadRequest("Entity's layout type should not be blank");
            }

            string type = "";
            switch (layoutType)
            {
                case LayoutType.List:
                    type = "List";
                    break;
                case LayoutType.CardView:
                    type = "CardView";
                    break;
            }

            if (string.IsNullOrEmpty(type))
            {
                return BadRequest("Invalid layout type");
            }

            string layoutFilePath = $"./Layout/{entity}/{type}.yaml";
            var dynamicYaml = System.IO.File.ReadAllText(layoutFilePath);
            var deserializer =  new DeserializerBuilder().Build();
            var yamlObject = deserializer.Deserialize<dynamic>(dynamicYaml);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(yamlObject, Newtonsoft.Json.Formatting.Indented);
            return Ok(json);
        }
    }
}