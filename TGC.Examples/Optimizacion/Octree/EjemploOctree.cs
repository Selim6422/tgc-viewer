using Microsoft.DirectX;
using System.Collections.Generic;
using TGC.Core.Example;
using TGC.Core.Geometries;
using TGC.Core.SceneLoader;
using TGC.Core.Terrain;
using TGC.Util;

namespace TGC.Examples.Optimizacion.Octree
{
    /// <summary>
    ///     Ejemplo EjemploOctree
    ///     Unidades Involucradas:
    ///     # Unidad 7 - Optimizaci�n - Octree
    ///     Muestra como crear y utilizar una Octree para optimizar el renderizado de un escenario por Frustum Culling.
    ///     El escenario es una isla con palmeras, rocas y el suelo. Solo las palmeras y rocas se optimizan con esta t�cnica.
    ///     Autor: Mat�as Leone, Leandro Barbagallo
    /// </summary>
    public class EjemploOctree : TgcExample
    {
        private List<TgcMesh> objetosIsla;
        private Octree octree;
        private TgcSkyBox skyBox;
        private TgcMesh terreno;

        public override string getCategory()
        {
            return "Optimizacion";
        }

        public override string getName()
        {
            return "Octree";
        }

        public override string getDescription()
        {
            return
                "Muestra como crear y utilizar una Octree para optimizar el renderizado de un escenario por Frustum Culling.";
        }

        public override void init()
        {
            //Crear SkyBox
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 500, 0);
            skyBox.Size = new Vector3(10000, 10000, 10000);
            var texturesPath = GuiController.Instance.ExamplesMediaDir + "Texturas\\Quake\\SkyBox LostAtSeaDay\\";
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "lostatseaday_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "lostatseaday_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "lostatseaday_lf.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "lostatseaday_rt.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "lostatseaday_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "lostatseaday_ft.jpg");
            skyBox.updateValues();

            //Cargar escenario de Isla
            var loader = new TgcSceneLoader();
            var scene =
                loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "Isla\\Isla-TgcScene.xml");

            //Separar el Terreno del resto de los objetos
            var list1 = new List<TgcMesh>();
            scene.separeteMeshList(new[] { "Terreno" }, out list1, out objetosIsla);
            terreno = list1[0];

            //Crear Octree
            octree = new Octree();
            octree.create(objetosIsla, scene.BoundingBox);
            octree.createDebugOctreeMeshes();

            //Camara en 1ra persona
            GuiController.Instance.FpsCamera.Enable = true;
            GuiController.Instance.FpsCamera.setCamera(new Vector3(1500, 800, 0), new Vector3(0, 0, -1));
            GuiController.Instance.FpsCamera.MovementSpeed = 500f;
            GuiController.Instance.FpsCamera.JumpSpeed = 500f;

            GuiController.Instance.Modifiers.addBoolean("showOctree", "Show Octree", false);
            GuiController.Instance.Modifiers.addBoolean("showTerrain", "Show Terrain", true);
        }

        public override void render(float elapsedTime)
        {
            var showOctree = (bool)GuiController.Instance.Modifiers["showOctree"];
            var showTerrain = (bool)GuiController.Instance.Modifiers["showTerrain"];

            skyBox.render();
            if (showTerrain)
            {
                terreno.render();
            }
            octree.render(TgcFrustum.Instance, showOctree);
        }

        public override void close()
        {
            skyBox.dispose();
            terreno.dispose();
            foreach (var mesh in objetosIsla)
            {
                mesh.dispose();
            }
        }
    }
}