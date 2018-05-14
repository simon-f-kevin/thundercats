using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Engine.Entities
{
    /* Methods for instantiating Entities with different sets of components. */
    public static class EntityFactory
    {
        public static Entity NewEntity()
        {
            return new Entity();
        }

        public static Entity NewEntity(string typeName)
        {
            return new Entity(typeName);
        }

        /* Adds all given components to a new Entity. */
        public static Entity NewEntityWithComponents(List<Component> components)
        {
            Entity entity = new Entity();

            for(int i = 0; i < components.Count; i++)
            {
                ComponentManager.Instance.AddComponentToEntity(entity, components[i]);
            }
            return new Entity();
        }

        /* Creates a new bounding box that contains all the models meshes. */
        public static BoundingBox CreateBoundingBox(Model model)
        {
            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            BoundingBox result = new BoundingBox();
            foreach(ModelMesh mesh in model.Meshes)
                foreach(ModelMeshPart meshPart in mesh.MeshParts)
                {
                    BoundingBox? meshPartBoundingBox = GetMeshBoundingBox(meshPart, boneTransforms[mesh.ParentBone.Index]);
                    if(meshPartBoundingBox != null)
                        result = BoundingBox.CreateMerged(result, meshPartBoundingBox.Value);
                }
            return result;
        }

        /* Adds four bounding boxes to the bounding hierarchy, each taking up a quadrant of the original bounding box. */
        public static void AddBoundingBoxChildren(BoundingBoxComponent boxComponent)
        {
            BoundingBox box = (BoundingBox)boxComponent.BoundingBox;
            Vector3[] corners = box.GetCorners();

            //May need some adjustments. Atm the min is the lowest X value of each corner. Might cause problems?
            //It would be more correct but more annoying to calculate the min for each sub-box instead of just using corners and the center.
            boxComponent.Children.Add(new BoundingBox(corners[0], boxComponent.Center));
            boxComponent.Children.Add(new BoundingBox(boxComponent.Center, corners[1]));
            boxComponent.Children.Add(new BoundingBox(boxComponent.Center, corners[2]));
            boxComponent.Children.Add(new BoundingBox(corners[3], boxComponent.Center));
        }

        /* Returns a new boudning box based on model mesh vertices. */
        private static BoundingBox? GetMeshBoundingBox(ModelMeshPart meshPart, Matrix transform)
        {
            if(meshPart.VertexBuffer == null)
                return null;

            Vector3[] positions = VertexElementExtractor.GetVertexElement(meshPart, VertexElementUsage.Position);
            if(positions == null)
                return null;

            Vector3[] transformedPositions = new Vector3[positions.Length];
            Vector3.Transform(positions, ref transform, transformedPositions);

            return BoundingBox.CreateFromPoints(transformedPositions);
        }

        private static class VertexElementExtractor
        {
            public static Vector3[] GetVertexElement(ModelMeshPart meshPart, VertexElementUsage usage)
            {
                VertexDeclaration vd = meshPart.VertexBuffer.VertexDeclaration;
                VertexElement[] elements = vd.GetVertexElements();

                Func<VertexElement, bool> elementPredicate = ve => ve.VertexElementUsage == usage && ve.VertexElementFormat == VertexElementFormat.Vector3;
                if(!elements.Any(elementPredicate))
                    return null;

                VertexElement element = elements.First(elementPredicate);

                Vector3[] vertexData = new Vector3[meshPart.NumVertices];
                meshPart.VertexBuffer.GetData((meshPart.VertexOffset * vd.VertexStride) + element.Offset,
                    vertexData, 0, vertexData.Length, vd.VertexStride);

                return vertexData;
            }
        }
    }
}
