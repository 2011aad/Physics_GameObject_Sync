# Synchronization of Physics Game Objects

For some reasons, if physical simulation runs only runs on several clients locally, even the game objects have the same initial state, the final state may be different. Therefore, in large scale MMORPG, if many game objects need to be handled by physics engine, the synchronization of these game objects becomes a problem. So, to eliminate the Butterfly Effect, we perform synchronization when collision occurs. Experiments show that this approach performs better than periodically synchronize game objects.
