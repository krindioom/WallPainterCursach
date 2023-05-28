#if ENABLE_PROFILER
using Unity.Collections;
using Unity.Profiling;

namespace Unity.Entities
{
    static partial class MemoryProfiler
    {
        [BurstCompatible(RequiredUnityDefine = "ENABLE_PROFILER")]
        public readonly struct BytesCounter
        {
            readonly ProfilerCounterValue<ulong> m_Counter;

            public ulong Value
            {
                get => m_Counter.Value;
                set => m_Counter.Value = value;
            }

            [NotBurstCompatible]
            public BytesCounter(string name)
            {
                m_Counter = new ProfilerCounterValue<ulong>(Category, name, ProfilerMarkerDataUnit.Bytes,
                    ProfilerCounterOptions.FlushOnEndOfFrame | ProfilerCounterOptions.ResetToZeroOnFlush);
            }
        }
    }
}
#endif
