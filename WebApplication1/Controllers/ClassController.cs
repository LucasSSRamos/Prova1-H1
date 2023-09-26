using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApplication1.Models;
using Newtonsoft.Json;


namespace WebApplication1.Controllers

{

    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        [HttpPost("ReceberCartas")]
        public IActionResult Class([FromBody] ClassModel carta)
        {
            if (carta == null)
            {
                return BadRequest("Dados inválidos.");
            }

            if (string.IsNullOrEmpty(carta.Nome) || carta.Nome.Length < 3 || carta.Nome.Length > 255)
            {
                return BadRequest("Nome é obrigatório e deve ter entre 3 e 255 caracteres.");
            }

            // Ler cartas existentes
            var cartas = LerCartas();

            cartas.Add(carta);

            // Salvar no arquivo JSON
            SalvarCartas(cartas);

            return Ok("Carta Enviada!");

        }

        [HttpGet("ListarCartas")]
        public IActionResult Get()
        {
            var cartas = LerCartas();
            return Ok(cartas);
        }

        private List<ClassModel> LerCartas()
        {
            if (System.IO.File.Exists(CartasFilePath))
            {
                var json = System.IO.File.ReadAllText(CartasFilePath);
                return JsonConvert.DeserializeObject<List<ClassModel>>(json);
            }
            else
            {
                return new List<ClassModel>();
            }
        }

        private void SalvarCartas(List<ClassModel> cartas)
        {
            var json = JsonConvert.SerializeObject(cartas);
            System.IO.File.WriteAllText(CartasFilePath, json);
        }
    }
}
