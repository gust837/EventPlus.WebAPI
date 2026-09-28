using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Utils;
using Microsoft.Extensions.Options;

namespace EventPlus.WebAPI.Service
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var credenciais = options.Value;

            //Account = "carteira" com as tres credenciais que autentificam na conta do cloudinary
            var account = new Account(credenciais.CloudName, credenciais.ApiKey, credenciais.ApiSecret);

            //Cria o cliente de fato ja autenticado com as credenciais
            _cloudinary = new Cloudinary(account);

            //Determina que as urls tem que vir com https
            _cloudinary.Api.Secure = true;
        }

        public async Task<string> UploadImagem(IFormFile arquivo)
        {
            //Abre um fluxo de leitura do arquivo
            //using garante que o stream será fechado após o uso (Libera memoria mesmo se der erro)
            using var stream = arquivo.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                //O arquivo em si nome original + o fluxo de bytes a enviar
                File = new FileDescription(arquivo.FileName, stream),
                //Pasta de destino dentro do Cloudnary
                Folder = "eventplus/eventos"
            };

            //Envia a imagem para o cloudinary e aguarda a resposta dos dados do upload 
            var resultado = await _cloudinary.UploadAsync(uploadParams);

            //Retorna a url segura da imagem
            return resultado.SecureUrl.AbsoluteUri;
        }
    }
}
