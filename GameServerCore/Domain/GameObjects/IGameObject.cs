using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Enums;

namespace GameServerCore.Domain.GameObjects
{
    /// <summary>
    /// Base class for all objects in League of Legends.
    /// GameObjects normally follow these guidelines of functionality: Position, Collision, Vision, Team, and Networking.
    /// </summary>
    public interface IGameObject : IUpdate
    {
        // Structure follows hierarchy of features (ex: an object is defined as collide-able before walk-able)

        /// <summary>
        ///  Identifier unique to this game object.
        /// </summary>
        uint NetId { get; }
        /// <summary>
        /// Radius of the circle which is used for collision detection between objects or terrain.
        /// </summary>
        float CollisionRadius { get; }
        /// <summary>
        /// Position of this GameObject from a top-down view.
        /// </summary>
        Vector2 Position { get; }
        /// <summary>
        /// Used to synchronize movement between client and server. Is currently assigned Env.TickCount.
        /// </summary>
        uint SyncId { get; }
        /// <summary>
        /// Team identifier, refer to TeamId enum.
        /// </summary>
        TeamId Team { get; }
        /// <summary>
        /// Radius of the circle which is used for vision; detecting if objects are visible given terrain, and if so, networked to the player (or team) that owns this game object.
        /// </summary>
        float VisionRadius { get; }

        /// <summary>
        /// Called by ObjectManager after AddObject (usually right after instatiation of GameObject).
        /// </summary>
        void OnAdded();

        /// <summary>
        /// Whether or not the object should be removed from the game (usually both server and client-side). Refer to ObjectManager.
        /// </summary>
        bool IsToRemove();

        /// <summary>
        /// Will cause ObjectManager to remove the object (usually) both server-side and client-side next update.
        /// </summary>
        void SetToRemove();

        /// <summary>
        /// Called by ObjectManager after the object has been SetToRemove.
        /// </summary>
        void OnRemoved();

        /// <summary>
        /// Sets the server-sided position of this object.
        /// </summary>
        void SetPosition(float x, float y);

        /// <summary>
        /// Sets the server-sided position of this object.
        /// </summary>
        void SetPosition(Vector2 vec);

        /// <summary>
        /// Refers to the height that the object is at in 3D space.
        /// </summary>
        float GetHeight();

        /// <summary>
        /// Gets the position of this GameObject in 3D space, where the Y value represents height.
        /// Mostly used for packets.
        /// </summary>
        /// <returns>Vector3 position.</returns>
        Vector3 GetPosition3D();

        /// <summary>
        /// Whether or not the specified object is colliding with this object.
        /// </summary>
        /// <param name="o">An object that could be colliding with this object.</param>
        bool IsCollidingWith(IGameObject o);

        /// <summary>
        /// Called by ObjectManager when the object is ontop of another object or when the object is inside terrain.
        /// </summary>
        void OnCollision(IGameObject collider, bool isTerrain = false);

        /// <summary>
        /// Sets the object's team.
        /// </summary>
        /// <param name="team">TeamId.BLUE/PURPLE/NEUTRAL</param>
        void SetTeam(TeamId team);
		
		/// <summary>
		/// Returns the vector which represents the 2d orientation that the object is moving in.
		/// </summary>
        Vector2 GetDirection();

        /// <summary>
        /// Whether or not the object is networked to a specified team.
        /// </summary>
        /// <param name="team">A team which could have vision of this object.</param>
        bool IsVisibleByTeam(TeamId team);

        /// <summary>
        /// Sets the object to be networked or not to a specified team.
        /// </summary>
        /// <param name="team">A team which could have vision of this object.</param>
        /// <param name="visible">true/false; networked or not</param>
        void SetVisibleByTeam(TeamId team, bool visible);

        /// <summary>
        /// Sets the position of this GameObject to the specified position.
        /// </summary>
        /// <param name="x">X coordinate to set.</param>
        /// <param name="y">Y coordinate to set.</param>
        void TeleportTo(float x, float y);
    }
}
