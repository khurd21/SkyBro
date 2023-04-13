using Alexa.NET.Request;
using Alexa.NET.Request.Type;

namespace WeatherObservations.Tests;

public class SkillRequestBuilder
{
    private SkillRequest SkillRequest { get; init; }

    public SkillRequestBuilder()
    {
        this.SkillRequest = new();
    }

    public SkillRequestBuilder WithIntent(string intentName)
    {
        this.SkillRequest.Request = new IntentRequest
        {
            Intent = new Intent
            {
                Name = intentName,
            },
        };

        return this;
    }

    public SkillRequestBuilder WithSlotResolutionId(string slotName, string slotId)
    {
        if (this.SkillRequest.Request == null || !(this.SkillRequest.Request is IntentRequest))
        {
            throw new InvalidOperationException($"Request must be an IntentRequest. Did you call ${nameof(this.WithIntent)} first?");
        }

        var slot = new Slot
        {
            Name = slotName,
            Resolution = new Resolution
            {
                Authorities = new[]
                {
                    new ResolutionAuthority
                    {
                        Values = new[]
                        {
                            new ResolutionValueContainer
                            {
                                Value = new ResolutionValue
                                {
                                    Id = slotId,
                                },
                            },
                        },
                    },
                },
            },
        };

        var intent = (IntentRequest)this.SkillRequest.Request;
        if (intent.Intent.Slots == null)
        {
            intent.Intent.Slots = new Dictionary<string, Slot>();
        }

        intent.Intent.Slots.Add(slotName, slot);
        this.SkillRequest.Request = intent;
        return this;
    }

    public SkillRequestBuilder WithSlotValue(string slotName, string slotValue)
    {
        var slot = new Slot
        {
            Name = slotName,
            SlotValue = new SlotValue
            {
                Value = slotValue,
            },
        };

        var intent = (IntentRequest)this.SkillRequest.Request;
        if (intent.Intent.Slots == null)
        {
            intent.Intent.Slots = new Dictionary<string, Slot>();
        }

        intent.Intent.Slots.Add(slotName, slot);
        this.SkillRequest.Request = intent;
        return this;
    }

    public SkillRequest Build()
    {
        return this.SkillRequest;
    }
}