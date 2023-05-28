using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using NUnit.Framework;
using Unity.Collections;
using Unity.CompilationPipeline.Common.Diagnostics;
using Unity.Entities.CodeGen.Tests.TestTypes;
using Unity.Entities.Serialization;

namespace Unity.Entities.CodeGen.Tests
{
    [TestFixture]
    public class BlobAssetSafetyVerifierTests : PostProcessorTestBase
    {
        public struct MyBlob
        {
            public BlobArray<float> myfloats;
        }

        class StoreBlobAssetReferenceValueInLocal_Class
        {
            static BlobAssetReference<MyBlob> _blobAssetReference;

            void Method()
            {
                MyBlob blob = _blobAssetReference.Value;
                EnsureNotOptimizedAway(blob.myfloats.Length);
            }
        }

        [Test]
        public void StoreBlobAssetReferenceValueInLocal()
        {
            AssertProducesError(
                typeof(StoreBlobAssetReferenceValueInLocal_Class),
                "error MayOnlyLiveInBlobStorageViolation: MyBlob may only live in blob storage. Access it by (non-readonly) ref instead: `ref MyBlob yourVariable = ref");
        }

        class LoadFieldFromBlobAssetReference_Class
        {
            static BlobAssetReference<MyBlob> _blobAssetReference;

            void Method()
            {
                BlobArray<float> myFloats = _blobAssetReference.Value.myfloats;
                EnsureNotOptimizedAway(myFloats.Length);
            }
        }

        [Test]
        public void LoadFieldFromBlobAssetReference()
        {
            AssertProducesError(
                typeof(LoadFieldFromBlobAssetReference_Class),
                " error MayOnlyLiveInBlobStorageViolation: You may only access .myfloats by (non-readonly) ref, as it may only live in blob storage. try `ref BlobArray<Single> yourVariable = ref");
        }

        class StoreBlobAssetReferenceValue_IntoReadonlyReference_Class
        {
            BlobAssetReference<MyBlob> _blobAssetReference;
            void Method()
            {
                ref readonly MyBlob readonlyBlob = ref _blobAssetReference.Value;
                EnsureNotOptimizedAway(readonlyBlob.myfloats.Length);
            }
        }

        [Test]
        public void StoreBlobAssetReferenceValue_IntoReadonlyReference()
        {
            AssertProducesError(
                typeof(StoreBlobAssetReferenceValue_IntoReadonlyReference_Class),
                "error MayOnlyLiveInBlobStorageViolation: You may only access .myfloats by (non-readonly) ref, as it may only live in blob storage. try `ref BlobArray<Single> yourVariable = ref");
        }

        class LoadFieldFromBlobAssetReference_IntoReadonlyReference_Class
        {
            BlobAssetReference<MyBlob> _blobAssetReference;
            void Method()
            {
                ref readonly BlobArray<float> myReadOnlyFloats = ref _blobAssetReference.Value.myfloats;
                EnsureNotOptimizedAway(myReadOnlyFloats.Length);
            }
        }

        [Test]
        public void LoadFieldFromBlobAssetReference_IntoReadonlyReference()
        {
            AssertProducesError(
                typeof(LoadFieldFromBlobAssetReference_IntoReadonlyReference_Class),
                "error MayOnlyLiveInBlobStorageViolation: BlobArray`1 may only live in blob storage. Access it by (non-readonly) ref instead: `ref BlobArray`1 yourVariable = ref");
        }

        class WithReferenceToValidType_Class
        {
            BoidInAnotherAssembly someField;
            void Method()
            {
                this.someField = new BoidInAnotherAssembly();
                EnsureNotOptimizedAway(this.someField);
            }
        }

        [Test]
        public void FailResolveWithWarning()
        {
            AssertProducesWarning(typeof(WithReferenceToValidType_Class),
                failResolve: true,
                "ResolveFailureWarning: Unable to resolve type Unity.Entities.CodeGen.Tests.TestTypes.BoidInAnotherAssembly for verification");
        }

        // Yield statements generate a state machine with unsafe use of blob assets
        // https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0ATEBqAPgAQCYBGAWACh8AGAAn2JQG4KL8BmOwmgYRoG8KNITQCuAOwDOAQwBmMOhwkAXKCLBKaAIQA2EYAEEoUKQE8APABUAfDQDuACxiwaFmiBrLV6wcL4BfH2FhQI8VNQ0dPW4IMSUpAEsxRIBzAGUw9QB1eKV7AFkYXIgMbNyATXiYbQwAfXSvJRCBciCgyIMjUzNEpRt2w2MTZnIQoPY6YjZu2JsKqowACgBKUd9V1oniOgB2GiphjaEAluFjoJDx/BQaPOX+EOO/IA===
        struct BlobContainingStructWithMethodWithYield_Struct
        {
            BlobArray<int> BlobArray;

            public IEnumerable<int> Method()
            {
                yield return 0;
            }
        }

        [Test]
        public void BlobContainingStructWithMethodWithYield_GeneratesError()
        {
            AssertProducesError(
                typeof(BlobContainingStructWithMethodWithYield_Struct),
                "error MayOnlyLiveInBlobStorageViolation: BlobContainingStructWithMethodWithYield_Struct may only live in blob storage. Access it by (non-readonly) ref instead");
        }

        class ClassWithValidBlobReferenceUsage
        {
            public class GenericTypeWithVolatile<T>
            {
                public volatile T[] buffer;
                public T this[int i] { get { return buffer[i]; } set { buffer[i] = value; } }
            }
            GenericTypeWithVolatile<int> _intGeneric;
            BoidInAnotherAssembly _someField;
            BlobAssetReference<MyBlob> _blobAssetReference;

            void Method()
            {
                _intGeneric = new GenericTypeWithVolatile<int>();
                _intGeneric.buffer = new[] {32, 12, 41};
                EnsureNotOptimizedAway(_intGeneric.buffer);

                _someField = new BoidInAnotherAssembly();
                EnsureNotOptimizedAway(_someField);

                ref BlobArray<float> myFloats = ref _blobAssetReference.Value.myfloats;
                EnsureNotOptimizedAway(myFloats.Length);

                ref MyBlob blob = ref _blobAssetReference.Value;
                EnsureNotOptimizedAway(blob.myfloats.Length);
            }
        }

        [Test]
        public void ValidBlobReferenceUsageSucceeds()
        {
            AssertProducesNoError(typeof(ClassWithValidBlobReferenceUsage));
        }

        struct ManagedBlob
        {
            public string s;
        }

        class WithManagedRefsInBlob_Class
        {
            void Method()
            {
                var builder = new BlobBuilder(Allocator.Temp);
                ref var root = ref builder.ConstructRoot<ManagedBlob>();
                root.s = "foo";
                EnsureNotOptimizedAway(root);
            }
        }

        [Test]
        public void ManagedRefsInBlobThrows()
        {
            AssertProducesError(
                typeof(WithManagedRefsInBlob_Class),
                "error ConstructBlobWithRefTypeViolation: You may not build a type ManagedBlob with Construct as ManagedBlob.s is a reference or pointer.  Only non-reference types are allowed in Blobs.");
        }

        unsafe struct BlobWithPointer
        {
            public int* p;
        }

        class WithPointerInBlob_Class
        {
            unsafe void Method()
            {
                var builder = new BlobBuilder(Allocator.Temp);
                ref var root = ref builder.ConstructRoot<BlobWithPointer>();
                root.p = null;
                EnsureNotOptimizedAway(root);
            }
        }

        [Test]
        public void PointerInBlobThrows()
        {
            AssertProducesError(
                typeof(WithPointerInBlob_Class),
                "error ConstructBlobWithRefTypeViolation: You may not build a type BlobWithPointer with Construct as BlobWithPointer.p is a reference or pointer.  Only non-reference types are allowed in Blobs.");
        }

        struct UnmanagedPtrInBlobPtr
        {
            public BlobPtr<BlobWithPointer> ptr;
        }

        class WithUnmanagedPtrInBlobPtr_Class
        {
            unsafe void Method()
            {
                var builder = new BlobBuilder(Allocator.Temp);
                ref var root = ref builder.ConstructRoot<UnmanagedPtrInBlobPtr>();
                ref var blobWithPtr = ref builder.Allocate(ref root.ptr);
                blobWithPtr.p = null;
                EnsureNotOptimizedAway(ref root);
            }
        }

        [Test]
        public void NestedPointerInBlobThrows()
        {
            AssertProducesError(
                typeof(WithUnmanagedPtrInBlobPtr_Class),
                "error ConstructBlobWithRefTypeViolation: You may not build a type UnmanagedPtrInBlobPtr with Construct as UnmanagedPtrInBlobPtr.ptr.Value.p is a reference or pointer.  Only non-reference types are allowed in Blobs.");
        }

        struct ManagedRefInBlobArray
        {
            public BlobArray<ManagedBlob> array;
        }

        class WithManagedRefInBlobArray_Class
        {
            unsafe void Method()
            {
                var builder = new BlobBuilder(Allocator.Temp);
                ref var root = ref builder.ConstructRoot<ManagedRefInBlobArray>();
                builder.Construct(ref root.array, new[] {new ManagedBlob {s = "foo"}});
                EnsureNotOptimizedAway(ref root);
            }
        }

        [Test]
        public void ManagedRefInBlobArrayThrows()
        {
            AssertProducesError(
                typeof(WithManagedRefInBlobArray_Class),
                "error ConstructBlobWithRefTypeViolation: You may not build a type ManagedRefInBlobArray with Construct as ManagedRefInBlobArray.array[].s is a reference or pointer.  Only non-reference types are allowed in Blobs.");
        }

        struct ManagedRefInStaticField
        {
            public static string s_ManagedString;
            public int i;
        }

        class WithManagedRefInStaticField_Class
        {
            void Method()
            {
                var builder = new BlobBuilder(Allocator.Temp);
                ref var root = ref builder.ConstructRoot<ManagedRefInStaticField>();
                root.i = 42;
                EnsureNotOptimizedAway(ref root);
            }
        }

        [Test]
        public void ManagedRefInStaticField_Doesnt_Throw()
        {
            AssertProducesNoError(typeof(WithManagedRefInStaticField_Class));
        }

        class WithGenericMethod_Class
        {
            BlobAssetReference<T> DoThing<T>() where T : struct
            {
                var builder = new BlobBuilder(Allocator.Temp);
                ref var root = ref builder.ConstructRoot<T>();
                EnsureNotOptimizedAway(ref root);
                return builder.CreateBlobAssetReference<T>(Allocator.Temp);
            }
        }

        [Test]
        public void GenericCode_CanCreateBlobAssetReference()
        {
            AssertProducesNoError(typeof(WithGenericMethod_Class));
        }

        unsafe struct BlobWithWeakAssetRef
        {
            public EntityPrefabReference PrefabRef;
        }

        class WithWeakAssetRefInBlob_Class
        {
            unsafe void Method()
            {
                var builder = new BlobBuilder(Allocator.Temp);
                ref var root = ref builder.ConstructRoot<BlobWithWeakAssetRef>();
                root.PrefabRef = default;
                EnsureNotOptimizedAway(root);
            }
        }

        [Test]
        public void WeakAssetRefInBlobThrows()
        {
            AssertProducesError(
                typeof(WithWeakAssetRefInBlob_Class),
                "error ConstructBlobWithRefTypeViolation: You may not build a type BlobWithWeakAssetRef with Construct as BlobWithWeakAssetRef.PrefabRef.PrefabId is an UntypedWeakReferenceId. Weak asset references are not yet supported in Blobs.");
        }

        void AssertProducesNoError(Type typeWithCodeUnderTest)
        {
            Assert.DoesNotThrow(() =>
            {
                var methodToAnalyze = MethodDefinitionForOnlyMethodOf(typeWithCodeUnderTest);
                var diagnosticMessages = new List<DiagnosticMessage>();

                try
                {
                    var verifyDiagnosticMessages = BlobAssetSafetyVerifier.VerifyMethod(methodToAnalyze, new HashSet<TypeReference>());
                    diagnosticMessages.AddRange(verifyDiagnosticMessages);
                }
                catch (FoundErrorInUserCodeException exc)
                {
                    diagnosticMessages.AddRange(exc.DiagnosticMessages);
                }

                Assert.AreEqual(0, diagnosticMessages.Count);
            });
        }

        protected override void AssertProducesInternal(
            Type typeWithCodeUnderTest,
            DiagnosticType diagnosticType,
            string[] shouldContains,
            bool failResolve = false)
        {
            var methodToAnalyze = MethodDefinitionForOnlyMethodOf(typeWithCodeUnderTest, failResolve);
            var diagnosticMessages = new List<DiagnosticMessage>();

            try
            {
                var verifyDiagnosticMessages = BlobAssetSafetyVerifier.VerifyMethod(methodToAnalyze, new HashSet<TypeReference>());
                diagnosticMessages.AddRange(verifyDiagnosticMessages);
            }
            catch (FoundErrorInUserCodeException exc)
            {
                diagnosticMessages.AddRange(exc.DiagnosticMessages);
            }

            Assert.AreEqual(1, diagnosticMessages.Count);
            Assert.AreEqual(diagnosticType, diagnosticMessages.Single().DiagnosticType);

            StringAssert.Contains(shouldContains.Single(), diagnosticMessages.Single().MessageData);

            // Currently BlobAsset errors can be generated by yield statements in a method.
            // In that case the method can contain no sequence points (and thus have no file info).
            //AssertDiagnosticHasSufficientFileAndLineInfo(diagnosticMessages);
        }

        void AssertProducesWarning(Type systemType, bool failResolve, params string[] shouldContainErrors)
        {
            AssertProducesInternal(systemType, DiagnosticType.Warning, shouldContainErrors, failResolve);
        }
    }
}
