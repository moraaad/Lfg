using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace LFG.Shared;

public class ImmagineVarianteFileStorage : ITransientDependency
{
    public const long MaxFileSizeBytes = 5 * 1024 * 1024;

    private const string CartellaRelativa = "/images/varianti/";

    private readonly IWebHostEnvironment _env;

    public ImmagineVarianteFileStorage(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string?> ValidaAsync(IFormFile file)
    {
        if (file.Length == 0)
        {
            return $"\"{file.FileName}\" è vuoto.";
        }

        if (file.Length > MaxFileSizeBytes)
        {
            return $"\"{file.FileName}\" supera la dimensione massima di 5 MB.";
        }

        var estensione = await RilevaEstensioneRealeAsync(file);
        if (estensione == null)
        {
            return $"\"{file.FileName}\" non è un'immagine JPG, PNG o WEBP valida.";
        }

        return null;
    }

    public async Task<string> SalvaAsync(IFormFile file)
    {
        var estensione = await RilevaEstensioneRealeAsync(file)
            ?? throw new InvalidOperationException("File non valido: chiamare ValidaAsync prima di SalvaAsync.");

        var nomeFile = $"{Guid.NewGuid()}{estensione}";
        var cartellaFisica = Path.Combine(_env.WebRootPath, "images", "varianti");
        Directory.CreateDirectory(cartellaFisica);

        var percorsoFisico = Path.Combine(cartellaFisica, nomeFile);
        using (var stream = new FileStream(percorsoFisico, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return CartellaRelativa + nomeFile;
    }

    public void Elimina(string? urlRelativo)
    {
        if (string.IsNullOrWhiteSpace(urlRelativo) ||
            !urlRelativo.StartsWith(CartellaRelativa, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var nomeFile = Path.GetFileName(urlRelativo);
        var percorsoFisico = Path.Combine(_env.WebRootPath, "images", "varianti", nomeFile);

        if (File.Exists(percorsoFisico))
        {
            File.Delete(percorsoFisico);
        }
    }

    private static async Task<string?> RilevaEstensioneRealeAsync(IFormFile file)
    {
        var buffer = new byte[12];
        using (var stream = file.OpenReadStream())
        {
            var letti = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (letti < 4)
            {
                return null;
            }
        }

        if (buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF)
        {
            return ".jpg";
        }

        if (buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47 &&
            buffer[4] == 0x0D && buffer[5] == 0x0A && buffer[6] == 0x1A && buffer[7] == 0x0A)
        {
            return ".png";
        }

        if (buffer[0] == 0x52 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x46 &&
            buffer[8] == 0x57 && buffer[9] == 0x45 && buffer[10] == 0x42 && buffer[11] == 0x50)
        {
            return ".webp";
        }

        return null;
    }
}
