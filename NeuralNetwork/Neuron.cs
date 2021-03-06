﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public enum NeuronType
    {
        Input,
        Hidden,
        Output,
        Bias,
        None
    }

    public class Neuron
    {
        public string Id { get; set; }
        public NeuronType Type { get; set; }

        public double Output { get; set; }
        public double NewOutput { get; set; }
        
        [JsonIgnore]
        public List<Connection> IncomingConnections { get; set; }
        [JsonIgnore]
        public List<Connection> OutgoingConnections { get; set; }

        public Neuron()
        {
            IncomingConnections = new List<Connection>();
            OutgoingConnections = new List<Connection>();

            Output = 0.0;
            NewOutput = 0.0;
        }

        /*
         * This constructor creates a copy of a Neuron
         * It needs a list of copied Connections to fill the
         *  IncomingConnections and OutgoingConnections lists
         * with copies of the original Connections
         */
        public Neuron(Neuron neuron, List<Connection> copiedConnections) : this()
        {
            Id = neuron.Id;
            Type = neuron.Type;
            //Output = neuron.Output;
            //NewOutput = neuron.NewOutput;
            foreach (Connection incomingConnection in neuron.IncomingConnections)
            {
                IncomingConnections.Add(copiedConnections.Find(connection => connection.Id == incomingConnection.Id));
            }
            foreach (Connection outgoingConnection in neuron.OutgoingConnections)
            {
                OutgoingConnections.Add(copiedConnections.Find(connection => connection.Id == outgoingConnection.Id));
            }
        }

        public Neuron(string id, NeuronType type) : this()
        {
            Id = id;
            Type = type;
        }

        public void UpdateNewOutput(DelegateActivationFunction activationFunction)
        {
            double sum = 0.0;

            // Calculate the sum of every incoming Connections weight times the incoming Neurons output
            for (int incoming = 0; incoming < IncomingConnections.Count; incoming++)
            {
                Connection incomingConnection = IncomingConnections[incoming];
                sum += incomingConnection.Weight * incomingConnection.NeuronFrom.Output;
            }

            // Calculate the Neurons new output by passing the sum through an ActivationFunction
            NewOutput = activationFunction(sum);
        }

        public override string ToString()
        {
            return ToJson().ToString();
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json.Add("Id", Id);
            json.Add("Type", Type.ToString());
            json.Add("Output", Output);
            json.Add("NewOutput", NewOutput);

            return json;
        }
    }
}
