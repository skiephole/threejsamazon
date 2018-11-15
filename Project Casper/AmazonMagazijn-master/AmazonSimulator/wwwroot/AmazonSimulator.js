const LoadStates = Object.freeze
	(
	{
		"NOT_LOADING": 1,
		"LOADING": 2,
		"LOADED": 3
	}
	);

function parseCommand(input = "")
{
	return JSON.parse(input);
}

var exampleSocket;

window.onload = function ()
{

	var camera, scene, renderer;
	var cameraControls;

	var worldObjects = {};

	function init()
	{
		camera = new THREE.PerspectiveCamera(70, window.innerWidth / window.innerHeight, 1, 1000);
		cameraControls = new THREE.OrbitControls(camera);
		camera.position.z = 15;
		camera.position.y = 5;
		camera.position.x = 15;
		cameraControls.update();
		scene = new THREE.Scene();

		renderer = new THREE.WebGLRenderer({ antialias: true });
		renderer.setPixelRatio(window.devicePixelRatio);
		renderer.setSize(window.innerWidth, window.innerHeight + 5);
		document.body.appendChild(renderer.domElement);

		window.addEventListener('resize', onWindowResize, false);

		var geometry = new THREE.PlaneGeometry(70, 60, 70);
		var material = new THREE.MeshPhongMaterial({ color: 0x6e7d93, side: THREE.DoubleSide });
		var plane = new THREE.Mesh(geometry, material);
		plane.rotation.x = Math.PI / 2.0;
		plane.position.x = 0;
		plane.position.z = 0;
		plane.position.y = 0;
		plane.recieveShadow = true;
		plane.castShadow = true;
		scene.add(plane);

		var sphericalSkyboxGeometry = new THREE.SphereGeometry(200, 64, 64);
		var sphericalSkyboxMaterial = new THREE.MeshBasicMaterial({ map: new THREE.TextureLoader().load("textures/Skybox/fridge.jpg"), side: THREE.DoubleSide });
		var SphericalBox = new THREE.Mesh(sphericalSkyboxGeometry, sphericalSkyboxMaterial);
		scene.add(SphericalBox);

		var light = new THREE.DirectionalLight(0xffffff);
		light.position.set(15, 10, 15);
		light.target.position.set(0, 0, 0);
		light.castShadow = true;
		light.shadowDarkness = 0.5;
		light.shadowCameraNear = 2;
		light.shadowCameraFar = 5;
		light.shadowCameraLeft = -0.5;
		light.shadowCameraRight = 0.5;
		light.shadowCameraTop = 0.5;
		light.shadowCameraBottom = -0.5;
		scene.add(light);

	}

	function onWindowResize()
	{
		camera.aspect = window.innerWidth / window.innerHeight;
		camera.updateProjectionMatrix();
		renderer.setSize(window.innerWidth, window.innerHeight);
	}

	function animate()
	{
		requestAnimationFrame(animate);
		cameraControls.update();
		renderer.render(scene, camera);
	}

	exampleSocket = new WebSocket("ws://" + window.location.hostname + ":" + window.location.port + "/connect_client");
	exampleSocket.onmessage = function (event)
	{
		var command = parseCommand(event.data);

		if (command.command = "update")
		{
			if (Object.keys(worldObjects).indexOf(command.parameters.guid) < 0)
			{
				if (command.parameters.type == "robot")
				{
					console.log(command);
					robot = new gRobot();

					var group = new THREE.Group();
					group.add(robot);

					scene.add(group);
					worldObjects[command.parameters.guid] = group;
				}
			}

			if (Object.keys(worldObjects).indexOf(command.parameters.guid) < 0)
			{
				if (command.parameters.type == "thunderhawk")
				{
					console.log(command);
					th = new gThunderHawk();

					var group = new THREE.Group();
					group.add(th);

					scene.add(group);
					worldObjects[command.parameters.guid] = group;
				}
			}

			if (Object.keys(worldObjects).indexOf(command.parameters.guid) < 0)
			{
				if (command.parameters.type == "shelf")
				{
					console.log(command);
					shelf = new gShelf();

					var group = new THREE.Group();
					group.add(shelf);

					scene.add(group);
					worldObjects[command.parameters.guid] = group;
				}
			}

			if (Object.keys(worldObjects).indexOf(command.parameters.guid) < 0)
			{
				if (command.parameters.type == "node")
				{
					console.log(command);
					node = new gNode();

					var group = new THREE.Group();
					group.add(node);

					scene.add(group);
					worldObjects[command.parameters.guid] = group;
				}
			}

			var object = worldObjects[command.parameters.guid];

			object.position.x = command.parameters.x;
			object.position.y = command.parameters.y;
			object.position.z = command.parameters.z;

			object.rotation.x = command.parameters.rotationX;
			object.rotation.y = command.parameters.rotationY;
			object.rotation.z = command.parameters.rotationZ;
		}
	}

	init();
	animate();
}
