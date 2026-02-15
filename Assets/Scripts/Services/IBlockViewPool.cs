using MVP.View;

namespace Services
{
    public interface IBlockViewPool
    {
        BlockView Get();
        void Return(BlockView blockView);
        void WarmUp(int count);
    }
}
