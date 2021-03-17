// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Greeting.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Greet {
  public static partial class GreetingService
  {
    static readonly string __ServiceName = "Greet.GreetingService";

    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    static readonly grpc::Marshaller<global::Greet.GreetingRequest> __Marshaller_Greet_GreetingRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Greet.GreetingRequest.Parser));
    static readonly grpc::Marshaller<global::Greet.GreetingResponse> __Marshaller_Greet_GreetingResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Greet.GreetingResponse.Parser));
    static readonly grpc::Marshaller<global::Greet.GreetManyTimesRequest> __Marshaller_Greet_GreetManyTimesRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Greet.GreetManyTimesRequest.Parser));
    static readonly grpc::Marshaller<global::Greet.GreetManyTimesResponse> __Marshaller_Greet_GreetManyTimesResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Greet.GreetManyTimesResponse.Parser));
    static readonly grpc::Marshaller<global::Greet.LongGreetRequest> __Marshaller_Greet_LongGreetRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Greet.LongGreetRequest.Parser));
    static readonly grpc::Marshaller<global::Greet.LongGreetResponse> __Marshaller_Greet_LongGreetResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Greet.LongGreetResponse.Parser));

    static readonly grpc::Method<global::Greet.GreetingRequest, global::Greet.GreetingResponse> __Method_Greet = new grpc::Method<global::Greet.GreetingRequest, global::Greet.GreetingResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Greet",
        __Marshaller_Greet_GreetingRequest,
        __Marshaller_Greet_GreetingResponse);

    static readonly grpc::Method<global::Greet.GreetManyTimesRequest, global::Greet.GreetManyTimesResponse> __Method_GreetManyTimes = new grpc::Method<global::Greet.GreetManyTimesRequest, global::Greet.GreetManyTimesResponse>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GreetManyTimes",
        __Marshaller_Greet_GreetManyTimesRequest,
        __Marshaller_Greet_GreetManyTimesResponse);

    static readonly grpc::Method<global::Greet.LongGreetRequest, global::Greet.LongGreetResponse> __Method_LongGreet = new grpc::Method<global::Greet.LongGreetRequest, global::Greet.LongGreetResponse>(
        grpc::MethodType.ClientStreaming,
        __ServiceName,
        "LongGreet",
        __Marshaller_Greet_LongGreetRequest,
        __Marshaller_Greet_LongGreetResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Greet.GreetingReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for GreetingService</summary>
    public partial class GreetingServiceClient : grpc::ClientBase<GreetingServiceClient>
    {
      /// <summary>Creates a new client for GreetingService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public GreetingServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for GreetingService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public GreetingServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected GreetingServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected GreetingServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Greet.GreetingResponse Greet(global::Greet.GreetingRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Greet(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Greet.GreetingResponse Greet(global::Greet.GreetingRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Greet, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Greet.GreetingResponse> GreetAsync(global::Greet.GreetingRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GreetAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Greet.GreetingResponse> GreetAsync(global::Greet.GreetingRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Greet, null, options, request);
      }
      public virtual grpc::AsyncServerStreamingCall<global::Greet.GreetManyTimesResponse> GreetManyTimes(global::Greet.GreetManyTimesRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GreetManyTimes(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::Greet.GreetManyTimesResponse> GreetManyTimes(global::Greet.GreetManyTimesRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GreetManyTimes, null, options, request);
      }
      public virtual grpc::AsyncClientStreamingCall<global::Greet.LongGreetRequest, global::Greet.LongGreetResponse> LongGreet(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return LongGreet(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncClientStreamingCall<global::Greet.LongGreetRequest, global::Greet.LongGreetResponse> LongGreet(grpc::CallOptions options)
      {
        return CallInvoker.AsyncClientStreamingCall(__Method_LongGreet, null, options);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override GreetingServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new GreetingServiceClient(configuration);
      }
    }

  }
}
#endregion
