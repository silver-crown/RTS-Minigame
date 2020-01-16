
namespace moveen.descs {
    public interface IOrderableTick {
        int getOrder();
        void tick(float dt);
        void fixedTick(float dt);
        bool doParticipateInsUpdate();
        bool participateInPhysicsUpdate();
    }
}