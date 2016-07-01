using Microsoft.DirectX;
using System.Collections.Generic;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.UserControls;
using TGC.Core.UserControls.Modifier;
using TGC.Examples.Example;

namespace TGC.Examples.SceneLoader
{
    /// <summary>
    ///     Ejemplo EjemploInstanciasPalmeras
    ///     Unidades Involucradas:
    ///     # Unidad 3 - Conceptos Basicos de 3D - Mesh
    ///     # Unidad 7 - Tecnicas de Optimizacion - Instancias de Modelos
    ///     Muestra como crear varias instancias de un mismo TgcMesh.
    ///     De esta forma se reutiliza su informacion grafica (triangulos, vertices, textura, etc).
    ///     Autor: Matias Leone, Leandro Barbagallo
    /// </summary>
    public class EjemploInstanciasPalmeras : TGCExampleViewer
    {
        private List<TgcMesh> meshes;
        private TgcMesh palmeraOriginal;
        private TgcBox suelo;

        public EjemploInstanciasPalmeras(string mediaDir, string shadersDir, TgcUserVars userVars,
            TgcModifiers modifiers) : base(mediaDir, shadersDir, userVars, modifiers)
        {
            Category = "SceneLoader";
            Name = "Instancias Palmeras";
            Description = "Muestra como crear varias instancias de un mismo TgcMesh.";
        }

        public override void Init()
        {
            //Crear suelo
            var pisoTexture = TgcTexture.createTexture(D3DDevice.Instance.Device, MediaDir + "Texturas\\pasto.jpg");
            suelo = TgcBox.fromSize(new Vector3(500, 0, 500), new Vector3(2000, 0, 2000), pisoTexture);

            //Cargar modelo de palmera original
            var loader = new TgcSceneLoader();
            var scene =
                loader.loadSceneFromFile(MediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
            palmeraOriginal = scene.Meshes[0];

            //Crear varias instancias del modelo original, pero sin volver a cargar el modelo entero cada vez
            var rows = 5;
            var cols = 6;
            float offset = 200;
            meshes = new List<TgcMesh>();
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    //Crear instancia de modelo
                    var instance = palmeraOriginal.createMeshInstance(palmeraOriginal.Name + i + "_" + j);

                    //Desplazarlo
                    instance.move(i * offset, 70, j * offset);
                    instance.Scale = new Vector3(0.25f, 0.25f, 0.25f);

                    meshes.Add(instance);
                }
            }

            //Camara en primera persona
            Camara = new TgcFpsCamera(new Vector3(61.8657f, 403.7024f, -527.558f), Input);
        }

        public override void Update()
        {
            PreUpdate();
        }

        public override void Render()
        {
            PreRender();

            //Renderizar suelo
            suelo.render();

            //Renderizar instancias
            foreach (var mesh in meshes)
            {
                mesh.render();
            }

            PostRender();
        }

        public override void Dispose()
        {
            suelo.dispose();

            //Al hacer dispose del original, se hace dispose automaticamente de todas las instancias
            palmeraOriginal.dispose();
        }
    }
}