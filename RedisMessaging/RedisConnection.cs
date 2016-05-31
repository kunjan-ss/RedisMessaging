﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using MessageQueue.Contracts;
using StackExchange.Redis;

namespace RedisMessaging
{
  public class RedisConnection : IConnection
  {
    private readonly string _connectionString;

    private static Lazy<ConnectionMultiplexer> _lazyConnection;

    public RedisConnection(string connectionString)
    {
      _connectionString = connectionString;
      IsConnected = true;
       _lazyConnection = new Lazy<ConnectionMultiplexer>(
      () =>
      {
        return ConnectionMultiplexer.Connect(_connectionString);
      });

    }

    //need a way to turn this on/off
    /// <summary>
    /// Endpoint of the backend storage option
    /// </summary>
    public virtual string Endpoint { get; private set; }

    /// <summary>
    /// Username to connect to the Enpoint
    /// </summary>
    public virtual string User
    {
      get
      {
        throw new NotSupportedException("Redis doesn't use usernames for authentication.");
      }
    }

    /// <summary>
    /// Password to connect to the Endpoint
    /// </summary>
    public virtual string Pass { get; private set; }

    public bool IsConnected { get; private set; }

    internal IConnectionMultiplexer Multiplexer { get { return _lazyConnection.Value; } }

    public void Connect()
    {
      if (IsConnected)
        return;

      IsConnected = true;
      //var connString = _connectionString;

      //if (!String.IsNullOrEmpty(Pass))
      //  connString =connString + ",password=" + Pass;

      //Multiplexer = ConnectionMultiplexer.Connect(connString);


      //if (Multiplexer.IsConnected)
      //{
      //  IsConnected = true;
      //}
      //else
      //{
      //  throw new Exception("multiplexer cannot connect to endpoint");
      //}
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      Multiplexer.Dispose();
      IsConnected = false;
    }
  }
}
