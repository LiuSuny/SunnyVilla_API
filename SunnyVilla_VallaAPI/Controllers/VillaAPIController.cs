using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SunnyVilla_VallaAPI.Data;
//using SunnyVilla_VallaAPI.Loggin;
using SunnyVilla_VallaAPI.Models;
using SunnyVilla_VallaAPI.Models.Dto;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace SunnyVilla_VallaAPI.Controllers
{
    //[Route("api/[Controller]")] // [Route ("api/VillaAPI")] are the same thing 
    [Route("api/VillaAPI")] //Note: we can use //without adding these we run into error in our program.cs
    [ApiController] //these tell our valla class that it is na API Controller
    public class VillaAPIController : ControllerBase
    {
        //private readonly ILogger<VillaAPIController> _logger; //dependent injection
        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //{
        //    _logger = logger; //Implement the dependent injection
        //}


        //private readonly ILogging _logger;//dependent injection using interface we created 
        //public VillaAPIController(ILogging logger)
        //{
        //    _logger = logger;
        //}

        public VillaAPIController()
        {
            
        }

        [HttpGet] //these notify the Ienumerable this is a get endpoint
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList); //Ok return 200k response type

        }


        [HttpGet("{id:int}", Name = "GetVilla")] //these notify the VillaDTO GetVilla(int id) this is a get endpoint of Id parameter 
        [ProducesResponseType(StatusCodes.Status200OK)] //these is specific
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(200, Type = typeof(VillaDTO))] //these is specific
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]

        //Next if we want to fetch a particular product from our store using Id, we can not use the HttpGet as it fetch everything
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
              
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);//these return product using link to id only
            if (villa == null)
            {
                return NotFound(); //404 
            }
            return Ok(villa);


        }

        [HttpPost] //httpPost to use when creating resource
        [ProducesResponseType(StatusCodes.Status201Created)] //these is specific
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreatedVilla(VillaDTO villaDTO)
        {
            //if(!ModelState.IsValid) //these validte our Api if name no enter etc
            //{
            //    return BadRequest(ModelState);
            //}
            //Next we check if our Villa name is unique 
            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exist"); //Customize validation
                return BadRequest(ModelState);
            }

            if (villaDTO == null) //this is for validation
            {
                return BadRequest(villaDTO);
            }
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); //these how to return custom error message
            }
            //next we retrieve our id and increment it by 1
            villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTO);

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
            //Next we initialize an API Delete

        }
        [ProducesResponseType(StatusCodes.Status204NoContent)] //these is specific
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);//these return product using link to id only
            if (villa == null)
            {
                return NotFound(); //404 
            }
            VillaStore.villaList.Remove(villa);
            return NoContent();
        }

        //Next we initialize an API Update which is httpPut or Patch 
        [ProducesResponseType(StatusCodes.Status204NoContent)] //these is specific
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            //if our id == null return 400 and check again if id is not exactly the Id inside our villDTO class models the still return 400
            if (id == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            //Trying to update our store
            villa.Name = villaDTO.Name;
            villa.SquarePerFeet = villaDTO.SquarePerFeet;
            villa.Occupancy = villaDTO.Occupancy;

            return NoContent();
        }

        //Next we initialize an JsonPatch Update which is httpPatch or Patch 
        [ProducesResponseType(StatusCodes.Status204NoContent)] //these is specific
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
       
        [HttpPatch("{id:int}", Name = "UpdatePatchVilla")]
        //Note: when working with httpPatch we recieve what they
        //call patch documents with instantiation JsonPatchDocument
        public IActionResult UpdatePatchVilla(int id, JsonPatchDocument <VillaDTO> patchDTO)
        {
            //if our id == null return 400 and check again if id is not exactly the Id inside our villDTO class models the still return 400
            if (patchDTO == null || id ==0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return BadRequest();
            }
            //Trying to updatePatch and jsonpatch needs to be updated and we need is  update villa obj
            //ApplyTo - is a jsonpatch method 
            patchDTO.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent(); //everything is correct we return no content
        }
    }
}
