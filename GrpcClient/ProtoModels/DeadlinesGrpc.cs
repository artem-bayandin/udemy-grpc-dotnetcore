// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Deadlines.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Deadlines {
  public static partial class DeadlineService
  {
    static readonly string __ServiceName = "Deadlines.DeadlineService";

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

    static readonly grpc::Marshaller<global::Deadlines.DeadlineRequest> __Marshaller_Deadlines_DeadlineRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Deadlines.DeadlineRequest.Parser));
    static readonly grpc::Marshaller<global::Deadlines.DeadlineResponse> __Marshaller_Deadlines_DeadlineResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Deadlines.DeadlineResponse.Parser));

    static readonly grpc::Method<global::Deadlines.DeadlineRequest, global::Deadlines.DeadlineResponse> __Method_DieOrNot = new grpc::Method<global::Deadlines.DeadlineRequest, global::Deadlines.DeadlineResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "DieOrNot",
        __Marshaller_Deadlines_DeadlineRequest,
        __Marshaller_Deadlines_DeadlineResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Deadlines.DeadlinesReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for DeadlineService</summary>
    public partial class DeadlineServiceClient : grpc::ClientBase<DeadlineServiceClient>
    {
      /// <summary>Creates a new client for DeadlineService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public DeadlineServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for DeadlineService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public DeadlineServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected DeadlineServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected DeadlineServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Deadlines.DeadlineResponse DieOrNot(global::Deadlines.DeadlineRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return DieOrNot(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Deadlines.DeadlineResponse DieOrNot(global::Deadlines.DeadlineRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_DieOrNot, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Deadlines.DeadlineResponse> DieOrNotAsync(global::Deadlines.DeadlineRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return DieOrNotAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Deadlines.DeadlineResponse> DieOrNotAsync(global::Deadlines.DeadlineRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_DieOrNot, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override DeadlineServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new DeadlineServiceClient(configuration);
      }
    }

  }
}
#endregion
