using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.ml.utilkit
{
    public class Improve
    {
        public static void instantaneousReinforce(NeuralEntity instance, Neuron[] neuronsThatShouldBeTrue, int repetitions)
        {
            instantaneousReinforce(instance, neuronsThatShouldBeTrue, repetitions, 0);
        }

        /**
         * Teaches the NN for an instantaneous, constant answer regarding a single
         * response. Useful for true-false situations
         * 
         * @param instance
         *            The NeuralEntity
         * @param neuronsThatShouldBeTrue
         *            Array of all neuons that should be equal to 1. The rest will try
         *            to be decreased to -1;
         * @param repetitions
         *            the amount of repetitions for the training.
         * @param chanceForSkip
         *            The chance that a neuron will be skipped. This is done to make
         *            sure neurons are not over specialized for each time the NN is
         *            reinforced.
         */
        public static void instantaneousReinforce(NeuralEntity instance, Neuron[] neuronsThatShouldBeTrue, int repetitions,
                double chanceForSkip)
        {
            Dictionary<Neuron, Double> h = new Dictionary<Neuron, Double>();
            foreach (Neuron n in instance.ai.getOutputNeurons())
            {
                bool isRightNeuron = false;
                foreach (Neuron rn in neuronsThatShouldBeTrue)
                {
                    if (n == rn)
                    {
                        isRightNeuron = true;
                        break;
                    }
                }
                h[n] = isRightNeuron ? 1.0 : -1.0;
            }
            instantaneousReinforce(instance, h, repetitions, chanceForSkip);
        }

        /**
         * Teaches the NN for an instantaneous, constant answer regarding a single
         * response. Useful for true-false situations
         * 
         * @param instance
         *            the NeuralEntity
         * @param correctValues
         *            a Dictionary of all neurons and the values they should be equal to
         * @param repetitions
         *            the amount of repetitions for the training.
         */
        public static void instantaneousReinforce(NeuralEntity instance, Dictionary<Neuron, Double> correctValues,
                int repetitions)
        {
            instantaneousReinforce(instance, correctValues, repetitions, 0);
        }

        /**
         * Teaches the NN for an instantaneous, constant answer regarding a single
         * response. Useful for true-false situations
         * 
         * @param instance
         *            the NeuralEntity
         * @param correctValues
         *            a Dictionary of all neurons and the values they should be equal to
         * @param repetitions
         *            the amount of repetitions for the training.
         * @param chanceForSkip
         *            The chance that a neuron will be skipped. This is done to make
         *            sure neurons are not over specialized for each time the NN is
         *            reinforced.
         * 
         */

        private static Random rnd = new Random();

        public static void instantaneousReinforce(NeuralEntity instance, Dictionary<Neuron, Double> correctValues, int repetitions,
                double chanceForSkip)
        {
            double step = 0.01;

            for (int loops = 0; loops < repetitions; loops++)
            {
                Dictionary<Neuron, Double> suggestedValueForNeuron = correctValues;
                // Subtract -1 a second time to not get the outputs
                // for (int layer = instance.ai.MAX_LAYERS - 1 - 1; layer >= 0; layer--)
                // {
                for (int layer = 0; layer < instance.ai.maxNeuralLayers - 1; layer++)
                {
                    Dictionary<Neuron, Double> lastOutputs = new Dictionary<Neuron, Double>();
                    // Since no new outputs are created, nor are any destroyed, we
                    // do not need to clear it: simply adding new values will
                    // override the existing vals.
                    foreach (Neuron n in instance.ai.getNeuronsInNeuralLayer(layer))
                    {
                        if (rnd.NextDouble() < chanceForSkip)
                            continue;

                        // Do thresh checks before if is not triggered

                        // Do thresholds after everything else
                        if (!n.isTriggered())
                        {
                            double orgThr = n.getThreshold();
                            foreach (Neuron outputs in suggestedValueForNeuron.Keys.ToList())
                                lastOutputs[outputs] = outputs.getTriggeredStength();

                            if (!(n is InputNeuron) && n.getTriggeredStength() <= n.getThreshold()) {
                n.setThreshold(-2);
                n.forceTriggerUpdateTree();
                double shouldLowerVal = 0;
                foreach (KeyValuePair<Neuron, Double> c in lastOutputs)
                {
                    double sug = suggestedValueForNeuron[c.Key];
                    if (c.Value > c.Key.getThreshold()
                            || c.Key.getTriggeredStength() > c.Key.getThreshold()
                            || c.Key.getTriggeredStength() > c.Value)
                        shouldLowerVal += (Math.Abs(c.Value - sug)
                                - Math.Abs(c.Key.getTriggeredStength() - sug));
                }
                n.setThreshold(orgThr);
                if (shouldLowerVal > 0)
                {
                    n.setThreshold(removeExtremes(n.getThreshold() - step, n.allowNegativeVals()));
                }
            }
            continue;
        }

					foreach (Neuron outputs in suggestedValueForNeuron.Keys.ToList())
						lastOutputs[outputs] = outputs.getTriggeredStength();

					int multiplier = rnd.NextDouble() > 0.5 ? -1 : 1;

        n.setWeight(n.getWeight() + (step* multiplier));
					n.forceTriggerUpdateTree();
					double change2 = 0.0;
					foreach (KeyValuePair<Neuron, Double> c in lastOutputs) {
						// TODO: Multipled by 100 so the change is not a small
						// number.
						double sug = suggestedValueForNeuron[c.Key]; // sug *= 100;
        int wasRightDirection = Math.Abs(sug - c.Key.getTriggeredStength()) <= Math
                .Abs(sug - c.Value) ? 1 : -1;

						if (c.Value > c.Key.getThreshold()
								|| c.Key.getTriggeredStength() > c.Key.getThreshold()
								|| c.Key.getTriggeredStength() > c.Value)
							change2 += wasRightDirection* Math.Abs(c.Key.getTriggeredStength() - c.Value);
        /*
         * change2 += ((Math .abs((c.Key.getTriggeredStength() * 100) -
         * (c.Value * 100))) * (Math .abs((suggestedValueForNeuron.get(c.Key)
         * - c .Key.getTriggeredStength())) <= Math
         * .abs(suggestedValueForNeuron.get(c.Key) - c.Value) ? 1.0 in -1.0));
         */

    }

					if (change2 == 0.0) {
						n.setWeight(n.getWeight() - (step* multiplier));
					} else if (change2< 0.0) {
						n.setWeight(n.getWeight() - (step* 2 * multiplier));
					}
					n.setWeight(removeExtremes(n.getWeight(), n.allowNegativeVals()));
					n.forceTriggerUpdateTree();

					foreach (int outputNeuronId in n.getStrengthIDs()) {
						Neuron outputNeuronInstance = n.getAI().neuronsByID(outputNeuronId);

						foreach (Neuron outputs in suggestedValueForNeuron.Keys.ToList())
							lastOutputs[outputs] = outputs.getTriggeredStength();

						int multiplier2 = rnd.NextDouble() > 0.5 ? -1 : 1;
n.setStrengthForNeuron(outputNeuronInstance,
        n.getOutputForNeuron(outputNeuronInstance) + (step* multiplier2));
						n.forceTriggerStengthUpdate();
						outputNeuronInstance.forceTriggerUpdateTree();
						double change = 0.0;
						foreach (KeyValuePair<Neuron, Double> c in lastOutputs) {
							double sug = suggestedValueForNeuron[c.Key];
int wasRightDirection = (Math.Abs(sug - c.Key.getTriggeredStength()) <= Math
        .Abs(sug - c.Value) ? 1 : -1);

							if (c.Value > c.Key.getThreshold()
									|| c.Key.getTriggeredStength() > c.Key.getThreshold()
									|| c.Key.getTriggeredStength() > c.Value)
								change += wasRightDirection* Math.Abs(c.Key.getTriggeredStength() - c.Value);
							/*
							 * change += ((Math.Abs((c.Key .getTriggeredStength() * 100) -
							 * (c.Value * 100))) * (Math .abs((suggestedValueForNeuron.get(c .Key)
							 * - c.Key .getTriggeredStength())) <= Math
							 * .abs(suggestedValueForNeuron.get(c.Key) - c.Value) ? 1.0 in -1.0));
							 */
						}

						if (change == 0.0) {
							n.setStrengthForNeuron(outputNeuronInstance,
                                    n.getOutputForNeuron(outputNeuronInstance) - (step* multiplier2));
						} else if (change< 0.0) {
							n.setStrengthForNeuron(outputNeuronInstance,
                                    n.getOutputForNeuron(outputNeuronInstance) - (step* 2 * multiplier2));
						}
						n.setStrengthForNeuron(outputNeuronInstance,
                                removeExtremes(n.getStrengthForNeuron(outputNeuronInstance), n.allowNegativeVals()));
						n.forceTriggerStengthUpdate();
						outputNeuronInstance.forceTriggerUpdateTree();
					}

					// Do check for inherit bias of neuron
					foreach (Neuron outputs in suggestedValueForNeuron.Keys.ToList())
						lastOutputs[outputs] = outputs.getTriggeredStength();
					int multiplier3 = rnd.NextDouble() > 0.5 ? -1 : 1;
n.setBias(n.getBias() + (step* multiplier3));
					n.forceTriggerUpdateTree();
					double change3 = 0.0;
					foreach (KeyValuePair<Neuron, Double> c in lastOutputs) {
						// TODO: Multipled by 100 so the change is not a small
						// number.
						double sug = suggestedValueForNeuron[c.Key];
int wasRightDirection = Math.Abs(sug - c.Key.getTriggeredStength()) <= Math
        .Abs(sug - c.Value) ? 1 : -1;

						if (c.Value > c.Key.getThreshold()
								|| c.Key.getTriggeredStength() > c.Key.getThreshold()
								|| c.Key.getTriggeredStength() > c.Value)
							change2 += wasRightDirection* Math.Abs(c.Key.getTriggeredStength() - c.Value);

					}
					// Step > 0 is already accounted for, since we already
					// deceased the step
					if (change3 == 0.0) {
						n.setBias(n.getBias() - (step* multiplier3));
					} else if (change3< 0.0) {
						n.setBias(n.getBias() - (step* 2 * multiplier3));
					}
					n.setBias(removeExtremes(n.getBias(), 50, n.allowNegativeVals()));
					n.forceTriggerUpdateTree();

					// Do Thresh checks after everything else if is triggered
					if (!(n is InputNeuron) && n.getThreshold() < n.getTriggeredStength()) {
						double orgThr = n.getThreshold();
						foreach (Neuron outputs in suggestedValueForNeuron.Keys.ToList())
							lastOutputs[outputs] = outputs.getTriggeredStength();

						n.setThreshold(2);
						n.forceTriggerUpdateTree();
						double shouldIncreaseVal = 0;
						foreach (KeyValuePair<Neuron, Double> c in lastOutputs) {
							double sug = suggestedValueForNeuron[c.Key];
							if (c.Value > c.Key.getThreshold()
									|| c.Key.getTriggeredStength() > c.Key.getThreshold()
									|| c.Key.getTriggeredStength() > c.Value)
								shouldIncreaseVal += (Math.Abs(c.Value - sug)
										- Math.Abs(c.Key.getTriggeredStength() - sug));
						}
						n.setThreshold(orgThr);
						if (shouldIncreaseVal > 0)
							n.setThreshold(removeExtremes(n.getThreshold() + step, n.allowNegativeVals()));
					}
				}
			}
		}
	}

	/**
	 * Teaches the NN for fluid, nonlinear responses given a set of multiple
	 * scenarios. Good for cases where you know true/false responses for certain
	 * cases, but not for all cases.
	 * 
	 * The reason this is done per-neuron is to allow for percentages to print out,
	 * or the ability to cancel the training if needed.
	 * 
	 * There will be no random checks for values. Values can sometimes get stuck in
	 * local minimums with no way out.
	 * 
	 * @param instance
	 *            the NeuralEntity
	 * @param n
	 *            The neuron that will be trained
	 * @param scenarios
	 *            The scenarios, which contain the inputs and the suggested output.
	 * 
	 */
	public static void multiScenarioReinforceNeuron(NeuralEntity instance, Neuron n, List<MemoryLossPrevent> scenarios)
{
    multiScenarioReinforceNeuron(instance, n, scenarios, 0.00);
}

/**
 * Teaches the NN for fluid, nonlinear responses given a set of multiple
 * scenarios. Good for cases where you know true/false responses for certain
 * cases, but not for all cases.
 * 
 * The reason this is done per-neuron is to allow for percentages to print out,
 * or the ability to cancel the training if needed.
 * 
 * @param instance
 *            the NeuralEntity
 * @param n
 *            The neuron that will be trained
 * @param scenarios
 *            The scenarios, which contain the inputs and the suggested output.
 * @param chanceForRandomValueCheck
 *            The chance (between 0.0 and 1.0) for the values for a neuron to be
 *            set to a random value and tested. Good for getting out of local
 *            minimums and finding better results
 * 
 */
public static void multiScenarioReinforceNeuron(NeuralEntity instance, Neuron n, List<MemoryLossPrevent> scenarios,
        double chanceForRandomValueCheck)
{
    double step = 0.01;
    // Subtract -1 a second time to not get the outputs
    // for (int layer = instance.ai.MAX_LAYERS - 1 - 1; layer >= 0; layer--)
    // {
    // Since no new outputs are created, nor are any destroyed, we
    // do not need to clear it: simply adding new values will
    // override the existing vals.

    // Do thresh checks before if is not triggered

    // Do thresholds after everything else
    n.setIsTraining(true);

    // if (!n.isTriggered()) {
    // TODO: Test if correct: Thresholds will not check inputs, however, inputs
    // should not be skipped.

    if (n.useThreshold())
    {
        double orgThr = n.getThreshold();
        if (!(n is InputNeuron)) {
            // Temporary check: If it is a bias neuron, reduce the threshold and invert the
            // weight
            // inverting weight should mean that whatever problem is was having before will
            // now be 'fixed'
            if (n is ByteNeuron && (n.getThreshold() >= 0.5)) {
                n.setWeight(-n.getWeight());
                n.setThreshold(0.3);
            }

            recordOutputs(instance, n, scenarios);
            n.setThreshold(-2);
            double difference = returnDifference(instance, n, scenarios);
            n.setThreshold(orgThr);
            if (difference > 0)
            {
                n.setThreshold(removeExtremes(n.getThreshold() - step, n.allowNegativeVals()));
            }
            else
            {
                recordOutputs(instance, n, scenarios);
                n.setThreshold(2);
                double difference4 = returnDifference(instance, n, scenarios);
                n.setThreshold(orgThr);
                if (difference4 > 0)
                {
                    n.setThreshold(removeExtremes(n.getThreshold() + step, n.allowNegativeVals()));
                }
            }
        } else {
            if (!everGetsActivated(n, scenarios))
            {
                n.setIsTraining(false);
                return;
            }
        }
    }
    // }



    if (!(n is InputNeuron || n is ByteNeuron)) {
        // Do check for inherit bias of neuron
        recordOutputs(instance, n, scenarios);
        if (rnd.NextDouble() <= chanceForRandomValueCheck)
        {

            double prevBoas = n.getBias();
            // Sets the bias between -50 and +50
            if (n.allowNegativeVals())
                n.setBias((rnd.NextDouble() * 4) - 2);
            else
                n.setBias((rnd.NextDouble() * 2));
            double difference3 = returnDifference(instance, n, scenarios);
            if (difference3 <= 0.0)
                n.setBias(prevBoas);
            n.setBias(removeExtremes(n.getBias(), 2, n.allowNegativeVals()));

        }
        else
        {
            int multiplier3 = rnd.NextDouble() > 0.5 ? -1 : 1;
            n.setBias(n.getBias() + (step * multiplier3));
            double difference3 = returnDifference(instance, n, scenarios);
            if (difference3 == 0.0)
            {
                n.setBias(n.getBias() - (step * multiplier3));
            }
            else if (difference3 < 0.0)
            {
                n.setBias(n.getBias() - (step * 2 * multiplier3));
            }
            n.setBias(removeExtremes(n.getBias(), 2, n.allowNegativeVals()));
        }
    }

    recordOutputs(instance, n, scenarios);

    if (rnd.NextDouble() <= chanceForRandomValueCheck)
    {

        double prevWeight = n.getWeight();
        if (n.allowNegativeVals())
            n.setWeight((rnd.NextDouble() * 2) - 1);
        else
            n.setWeight((rnd.NextDouble()));
        double difference = returnDifference(instance, n, scenarios);
        if (difference <= 0.0)
            n.setWeight(prevWeight);
        n.setWeight(removeExtremes(n.getWeight(), n.allowNegativeVals()));

    }
    else
    {
        int multiplier = rnd.NextDouble() > 0.5 ? -1 : 1;
        n.setWeight(n.getWeight() + (step * multiplier));
        double difference = returnDifference(instance, n, scenarios);
        if (difference == 0.0)
        {
            n.setWeight(n.getWeight() - (step * multiplier));
        }
        else if (difference < 0.0)
        {
            n.setWeight(n.getWeight() - (step * 2 * multiplier));
        }
        n.setWeight(removeExtremes(n.getWeight(), n.allowNegativeVals()));
    }

    foreach (int outputNeuronId in n.getStrengthIDs())
    {
        Neuron outputNeuronInstance = n.getAI().neuronsByID(outputNeuronId);

        if (outputNeuronInstance.droppedOut())
            continue;

        recordOutputs(instance, n, scenarios);

        if (rnd.NextDouble() <= chanceForRandomValueCheck)
        {

            double prevStength = n.getStrengthForNeuron(outputNeuronInstance);
            if (n.allowNegativeVals())
                n.setStrengthForNeuron(outputNeuronId, (rnd.NextDouble() * 2) - 1);
            else
                n.setStrengthForNeuron(outputNeuronId, (rnd.NextDouble()));
            double difference2 = returnDifference(instance, n, scenarios);
            if (difference2 <= 0.0)
                n.setStrengthForNeuron(outputNeuronId, prevStength);
            n.setStrengthForNeuron(outputNeuronInstance,
                    removeExtremes(n.getStrengthForNeuron(outputNeuronInstance), n.allowNegativeVals()));
        }
        else
        {
            int multiplier2 = rnd.NextDouble() > 0.5 ? -1 : 1;
            n.setStrengthForNeuron(outputNeuronInstance,
                    n.getOutputForNeuron(outputNeuronInstance) + (step * multiplier2));
            double difference2 = returnDifference(instance, n, scenarios);

            if (difference2 == 0.0)
            {
                n.setStrengthForNeuron(outputNeuronInstance,
                        n.getOutputForNeuron(outputNeuronInstance) - (step * multiplier2));
            }
            else if (difference2 < 0.0)
            {
                n.setStrengthForNeuron(outputNeuronInstance,
                        n.getOutputForNeuron(outputNeuronInstance) - (step * 2 * multiplier2));
            }
            n.setStrengthForNeuron(outputNeuronInstance,
                    removeExtremes(n.getStrengthForNeuron(outputNeuronInstance), n.allowNegativeVals()));
        }
    }
    n.setIsTraining(false);
}

public static bool everGetsActivated(Neuron n, List<MemoryLossPrevent> scenarios)
{
    foreach (MemoryLossPrevent mem in scenarios)
    {
        if (everGetsActivated(n, mem))
            return true;
    }
    return false;
}

public static bool everGetsActivated(Neuron n, MemoryLossPrevent scenario)
{
    if (scenario.needsToUse())
        return true;
    if (n.getLayer() > 0)
        return true;
    if (n is ByteNeuron)
			if (n.getThreshold() < 0.5)
        return true;
    else
        return false;
    if (scenario.inputValues.ContainsKey(n.getID()))
    {
        double newVal = scenario.inputValues[n.getID()];
        if (newVal > 0)
            return true;
    }
    return false;
}

public static void recordOutputs(NeuralEntity instance, Neuron testFor, List<MemoryLossPrevent> scenarios)
{

    foreach (MemoryLossPrevent mem in scenarios)
    {
        if (!everGetsActivated(testFor, mem))
            continue;
        foreach (Neuron inN in instance.ai.getInputNeurons())
        {
            if (mem.inputValues.ContainsKey(inN.getID()))
            {
                if (inN is ByteNeuron)
						continue;
    double newVal = mem.inputValues[inN.getID()];
    inN.forceLastResultChange(newVal);
    ((InputNeuron)inN).setIsTriggeredLast(newVal > 0);
}
			}
			instance.ai.ForceUpdateInLayer(1);

			Dictionary<int, Double> outputs = new Dictionary<int, Double>();

			foreach (Neuron n2 in instance.ai.getOutputNeurons()) {
				outputs[n2.getID()] = n2.getTriggeredStength();
			}
			mem.updatePreviousOutputs(outputs);
		}
	}

	public static double returnDifference(NeuralEntity instance, Neuron testFor, List<MemoryLossPrevent> scenarios)
{
    double difference = 0;
    foreach (MemoryLossPrevent mem in scenarios)
    {
        if (!everGetsActivated(testFor, mem))
            continue;
        foreach (Neuron inN in instance.ai.getInputNeurons())
        {
            if (inN is ByteNeuron)
					continue;
    double newVal = mem.inputValues[inN.getID()];
    inN.forceLastResultChange(newVal);
    ((InputNeuron)inN).setIsTriggeredLast(newVal > 0);
}
			instance.ai.ForceUpdateInLayer(1);

			Dictionary<int, Double> currentoutputs = new Dictionary<int, Double>();
			foreach (Neuron n2 in instance.ai.getOutputNeurons()) {
				currentoutputs[n2.getID()] = n2.getTriggeredStength();
			}

			foreach (KeyValuePair<int, Double> suggested in mem.suggestOutputValues) {
				// TODO: Check if correct: By adding or subtracting 1, sqaring the number should
				// never create a smaller number
				double deltaOrg = (suggested.Value - mem.previousOutputValues[suggested.Key]);
				if (deltaOrg< 0)

                    deltaOrg--;
				else
					deltaOrg++;
				double deltaDifference = (suggested.Value - currentoutputs[suggested.Key]);
				if (deltaDifference< 0)

                    deltaDifference--;
				else
					deltaDifference++;

				difference += (deltaOrg* deltaOrg) - (deltaDifference* deltaDifference);

				// TODO: Check if correct: Switching val make it positive, but on average
				// further than needed

				// difference += (suggested.Value -
				// mem.previousOutputValues.get(suggested.Key))
				// - (suggested.Value - currentoutputs.get(suggested.Key));
			}
		}
		return difference;
	}

	private static double removeExtremes(double d, bool allowNeg)
{
    if ((!allowNeg) && d < 0)
        return 0;
    if (d > 1)
        return 1;
    if (d < -1)
        return -1;
    return d;
}

private static double removeExtremes(double d, double max, bool allowNeg)
{
    if ((!allowNeg) && d < 0)
        return 0;
    if (d > max)
        return max;
    if (d < -max)
        return -max;
    return d;
}
    }
}
