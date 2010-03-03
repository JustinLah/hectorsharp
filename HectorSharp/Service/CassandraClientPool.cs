﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HectorSharp.Service
{
    /**
     * Holds the list of all available pools, keyed by their url:port.
     *
     * This class is a simple multiplexer b/w the different client pools, keyed by url:port.
     *
     * To obtain new CassandraClient object invoke borrowClient(). Once the application
     * is done, releaseClient().
     *
     *Example code:
     *
     * <pre>
     *
     * CassandraClient client = clientpool.borrowClient(hostname, port);
     * try {
     *   // do something with client and buessiness logic
     * } catch (Exception e) {
     *   // process exception
     * } finally {
     *   clientpool.releaseClient(client);
     * }
     * </pre>
     *
     * @author Ran Tavory (rantav@gmain.com)
     *
     */
    public interface CassandraClientPool
    {

        /**
         * Borrows a client from the pool defined by url:port
         * @param url
         * @param port
         * @return
         */
        CassandraClient borrowClient(String url, int port);

        /**
         * Borrows a client, similar to {@link #borrowClient(String, int)}, but expects the url:port
         * string format
         * @param urlPort a string of the format url:port
         */
        CassandraClient borrowClient(String urlPort);

        /**
         * Borrows a load-balanced client, a random client from the array of given client addresses.
         *
         * This method is typically used to allow load balancing b/w the list of given client URLs. The
         * method will return a random client from the array of the given url:port pairs.
         * The method will try connecting each host in the list and will only stop when there's one
         * successful connection, so in that sense it's also useful for failover.
         *
         * @param clientUrls An array of "url:port" cassandra client addresses.
         *
         * @return A randomly chosen client from the array of clientUrls.
         * @throws Exception
         */
        CassandraClient borrowClient(String[] clientUrls);

        /**
         * Releases a client from the pool it belongs to.
         */
        void releaseClient(CassandraClient client);

        /**
         * Returns the client associated with this keyspace to the connection pool.
         * This is just short for releaseClient(k.getClient());
         */
        void releaseKeyspace(Keyspace k);

        /**
         * Tells all the clients in the pool to update their list of known hosts.
         * @throws TException
         */
        void updateKnownHosts();

        Set<String> getExhaustedPoolNames();

        Set<String> getPoolNames();

        int getNumPools();

        int getNumIdle();

        int getNumExhaustedPools();

        int getNumBlockedThreads();

        int getNumActive();

        Set<String> getKnownHosts();

        /**
         * Use this method to invalidate the client and take it out of the pool.
         * This is usually so when the client has errors.
         */
        void invalidateClient(CassandraClient client);
    }
}
