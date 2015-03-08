#pragma strict
 
var terrainWidth:int; // How many vertices wide the terrain will be
 
var resolution:float; // How granular the terrain will be the higher
                      // the number, the closer together points will be
 
var roughness:float; // How rough the terrain will be
 
var startHeight:float; // How high the leftmost vertex will be
 
var endHeight:float; // How high the rightmost vertex will be
	
var xShift:float;

var yShift:float;
 
function Awake() {
  //Debug.Log("Generating Terrain");

  xShift = transform.position.x;
  yShift = transform.position.y;
  
  // Generate the heightmap
  var heightmap:float[] = GenerateHeightMap(startHeight, endHeight, terrainWidth);
  
  // Create vertices, uv's and triangles
  var terrainVertices:Vector2[] = CreateTerrainVertices(heightmap, resolution);
  var terrainUV:Vector2[] = GenerateTerrainUV(heightmap);  
  var terrainTriangles:int[] = Triangulate(terrainVertices.Length);
  
  // Create the mesh!
  GenerateMesh(terrainVertices, terrainTriangles, terrainUV, "ground", 0);
  
  // Repeat the process for grass
  var grassVertices:Vector2[] = CreateGrassVertices(heightmap, resolution);
  var grassUV:Vector2[] = GenerateGrassUV(heightmap);
  var grassTriangles:int[] = Triangulate(terrainVertices.Length);
  
  GenerateMesh(grassVertices, grassTriangles, grassUV, "grass", 0);
 
  //Debug.Log("Terrain Gen complete");

}
 
function GenerateHeightMap(startHeight:float, endHeight:float, count:int) {
  //Debug.Log("Generating heightmap");
  // Create a heightmap array and set the start and endpoints
  var heightmap:float[] = new float[count + 1];
  heightmap[0] = startHeight;
  heightmap[heightmap.Length - 1] = endHeight;
  
  // Call the recursive function to generate the heightmap
  GenerateMidPoint(0, heightmap.Length - 1, roughness, heightmap);
  //Debug.Log("Heightmap complete");
  return heightmap;
}
 
function GenerateMidPoint(start:int, end:int, roughness:float, heightmap:float[]) {
  // Find the midpoint of the array for this step
  var midPoint:int = Mathf.Floor((start + end) / 2);
  
  if (midPoint != start) {
    // Find the mid height for this step
    var midHeight = (heightmap[start] + heightmap[end]) / 2;
    
    // Generate a new displacement between the roughness factor
    heightmap[midPoint] = midHeight + Random.Range(-roughness, roughness);
    
    // Repeat the process for the left side and right side of
    // the new mid point
    GenerateMidPoint(start, midPoint, roughness / 2, heightmap);
    GenerateMidPoint(midPoint, end, roughness / 2, heightmap);
  }
}
 
function CreateTerrainVertices(heightmap:float[], resolution:float) {
  //Debug.Log("Creating terrain vertices");
  // The minimum resolution is 1
  resolution = Mathf.Max(1, resolution);
  
  var vertices:Array = new Array();
  
  // For each point, in the heightmap, create a vertex for
  // the top and the bottom of the terrain.
  for (var i:float = 0; i < heightmap.length; i += 1) {
    vertices.Push(new Vector2(i / resolution, heightmap[i]));
    vertices.Push(new Vector2(i / resolution, 0));
  }
 
  //Debug.Log("Created " + vertices.length + " terrain vertices");  
  return vertices.ToBuiltin(Vector2) as Vector2[];
}
 
function CreateGrassVertices(heightmap:float[], resolution:float) {
  //Debug.Log("Creating grass vertices");
  resolution = Mathf.Max(1, resolution);
  
  var vertices:Array = new Array();
  
  for (var i:float = 0; i < heightmap.length; i += 1) {
    vertices.Push(new Vector2(i / resolution, heightmap[i]));
    vertices.Push(new Vector2(i / resolution, heightmap[i] - 1));
  }
 
  //Debug.Log("Created " + vertices.length + " grass vertices");  
  return vertices.ToBuiltin(Vector2) as Vector2[];
}
 
function GenerateTerrainUV(heightmap:float[]) {
  //Debug.Log("Generating terrain UV co-ords");
 
  var uv:Array = new Array();
 
  var factor:float = 1.0f / heightmap.Length;
 
  // Loop through heightmap and create a UV point
  // for the top and bottom.
  for (var i:int = 0; i < heightmap.length; i++) {
    uv.Push(new Vector2((factor * i) * 20, heightmap[i] / 20));
    uv.Push(new Vector2((factor * i) * 20, 0));
  }
 
  //Debug.Log("Generated " + uv.length + " grass UV co-ords");  
  return uv.ToBuiltin(Vector2) as Vector2[];
}
 
function GenerateGrassUV(heightmap:float[]) {
  //Debug.Log("Generating grass UV co-ords");
 
  var uv:Array = new Array();
 
  var factor:float = 1.0f / heightmap.Length;
 
  for (var i:int = 0; i < heightmap.length; i++) {
    uv.Push(new Vector2(i % 2, 1));
    uv.Push(new Vector2(i % 2, 0));
  }
 
  //Debug.Log("Generated " + uv.length + " terrain UV co-ords");  
  return uv.ToBuiltin(Vector2) as Vector2[];
}
 
function GenerateMesh(vertices:Vector2[], triangles:int[], uv:Vector2[], texture:String, z:int) {
  //Debug.Log("Building Mesh");
  var meshVertices:Array = new Array();
 
  // Convert our Vector2's to Vector3
  for (var vertex:Vector2 in vertices) {
    meshVertices.Push(new Vector3(vertex.x+xShift, vertex.y+yShift, transform.position.z + z));
  }
  
  // Create a new mesh and set the vertices, uv's and triangles
  var mesh:Mesh = new Mesh();
  mesh.vertices = meshVertices;
  mesh.uv = uv;
  mesh.triangles = triangles;
  mesh.RecalculateNormals();
  mesh.RecalculateBounds();
  
  // Create a new game object
  var go:GameObject = new GameObject(texture);
  
  // Add the mesh to the object
  go.AddComponent(MeshRenderer);
  var filter:MeshFilter = go.AddComponent(MeshFilter);
  filter.mesh = mesh;

  // Add a collider

  go.AddComponent(MeshCollider);
 
  // Add a texture  
  go.renderer.material.mainTexture = Resources.Load(texture) as Texture;

  // Reparent as a child of this game object
  go.transform.parent = transform;
 
  //Debug.Log("Mesh built"); 
}
 
function Triangulate(count:int) {
  var indices:Array = new Array();
 
  // For each group of 4 vertices, add 6 indices
  // to create 2 triangles
  for (var i:int = 0; i < count - 4; i += 2) {
    indices.push(i);
    indices.push(i + 3);
    indices.push(i + 1);
 
    indices.push(i + 3);
    indices.push(i);
    indices.push(i + 2);
  }
 
  return indices.ToBuiltin(int) as int[];
}