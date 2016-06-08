using Microsoft.DirectX;
using System;
using System.Drawing;
using TGC.Core;
using TGC.Core.Camara;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Particle;
using TGC.Core.UserControls;
using TGC.Core.UserControls.Modifier;

namespace TGC.Examples.Particles
{
    /// <summary>
    ///     Emisor de Particulas
    /// </summary>
    public class EmisorDeParticulas : TgcExample
    {
        private TgcBox box;
        private ParticleEmitter emitter;
        private int selectedParticleCount;
        private string selectedTextureName;
        private string[] textureNames;
        private string texturePath;

        public EmisorDeParticulas(string mediaDir, string shadersDir, TgcUserVars userVars, TgcModifiers modifiers,
            TgcAxisLines axisLines, TgcCamera camara)
            : base(mediaDir, shadersDir, userVars, modifiers, axisLines, camara)
        {
            Category = "Particles";
            Name = "Emisor de Particulas";
            Description = "Emisor de Particulas";
        }

        public override void Init()
        {
            //Directorio de texturas
            texturePath = MediaDir + "Texturas\\Particles\\";

            //Texturas de particulas a utilizar
            textureNames = new[]
            {
                "pisada.png",
                "fuego.png",
                "humo.png",
                "hoja.png",
                "agua.png",
                "nieve.png"
            };

            //Modifiers
            Modifiers.addInterval("texture", textureNames, 0);
            Modifiers.addInt("cantidad", 1, 30, 10);
            Modifiers.addFloat("minSize", 0.25f, 10, 4);
            Modifiers.addFloat("maxSize", 0.25f, 10, 6);
            Modifiers.addFloat("timeToLive", 0.25f, 2, 1);
            Modifiers.addFloat("frecuencia", 0.25f, 4, 1);
            Modifiers.addInt("dispersion", 50, 400, 100);
            Modifiers.addVertex3f("speedDir", new Vector3(-50, -50, -50), new Vector3(50, 50, 50),
                new Vector3(30, 30, 30));

            //Crear emisor de particulas
            selectedTextureName = textureNames[0];
            selectedParticleCount = 10;
            emitter = new ParticleEmitter(texturePath + selectedTextureName, selectedParticleCount);
            emitter.Position = new Vector3(0, 0, 0);

            box = TgcBox.fromSize(new Vector3(0, -30, 0), new Vector3(10, 10, 10), Color.Blue);

            Camara = new TgcRotationalCamera(box.BoundingBox);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Render()
        {
            IniciarEscena();
            base.Render();

            //Cambiar cantidad de particulas, implica crear un nuevo emisor
            var cantidad = (int)Modifiers["cantidad"];
            if (selectedParticleCount != cantidad)
            {
                //Crear nuevo emisor
                selectedParticleCount = cantidad;
                emitter.dispose();
                emitter = new ParticleEmitter(texturePath + selectedTextureName, selectedParticleCount);
            }

            //Cambiar textura
            var textureName = (string)Modifiers["texture"];
            if (selectedTextureName != textureName)
            {
                selectedTextureName = textureName;
                emitter.changeTexture(texturePath + selectedTextureName);
            }

            //Actualizar los dem�s parametros
            emitter.MinSizeParticle = (float)Modifiers["minSize"];
            emitter.MaxSizeParticle = (float)Modifiers["maxSize"];
            emitter.ParticleTimeToLive = (float)Modifiers["timeToLive"];
            emitter.CreationFrecuency = (float)Modifiers["frecuencia"];
            emitter.Dispersion = (int)Modifiers["dispersion"];
            emitter.Speed = (Vector3)Modifiers["speedDir"];

            //Render de emisor
            emitter.render(ElapsedTime);

            box.render();

            FinalizarEscena();
        }

        public override void Close()
        {
            base.Close();

            //Liberar recursos
            emitter.dispose();

            box.dispose();
        }
    }
}