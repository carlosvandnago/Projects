using TaxRacm.Clients.Domain.Entities;
using TaxRacm.Clients.Domain.Enums;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.Clients.Infrastructure.Persistence;
using TaxRacm.Controls.Domain.Entities;
using TaxRacm.Controls.Domain.Enums;
using TaxRacm.Controls.Infrastructure.Persistence;
using TaxRacm.Risks.Domain.Entities;
using TaxRacm.Risks.Domain.Enums;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.Risks.Infrastructure.Persistence;

namespace TaxRacm.Api.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var clientsDb = services.GetRequiredService<ClientsDbContext>();
        var risksDb = services.GetRequiredService<RisksDbContext>();
        var controlsDb = services.GetRequiredService<ControlsDbContext>();

        if (risksDb.RiskBankEntries.Any()) return;

        var globalOwnerId = new UserId(Guid.NewGuid());

        // ── Risk Bank ─────────────────────────────────────────────────────────
        var riskBank = new List<RiskBankEntry>
        {
            RiskBankEntry.Create(
                "Transfer Pricing Documentation Gap",
                TaxType.TransferPricing,
                "Intercompany transactions lack contemporaneous documentation meeting OECD BEPS Action 13 standards.",
                new() { "Rapid organisational growth", "Lack of TP policy governance", "Decentralised finance teams" },
                new() { "Penalties up to 100% of unpaid tax", "Reputational damage", "Double taxation exposure" },
                new() { "Annual TP policy review", "Intercompany agreement register", "Benchmarking study mandate" },
                new() { "Local file preparation protocol", "Contemporaneous documentation sign-off" },
                4, 5,
                new() { "GB", "DE", "FR", "NL", "US", "SG" },
                new() { "Manufacturing", "Technology", "Financial Services" },
                new() { "BEPS", "OECD", "documentation" }),

            RiskBankEntry.Create(
                "Permanent Establishment Exposure",
                TaxType.CorporateIncomeTax,
                "Remote workers or sales agents may create undisclosed permanent establishments in foreign jurisdictions.",
                new() { "Remote working policies post-COVID", "Travelling executives with signing authority", "Digital services in market countries" },
                new() { "Unexpected corporate tax liability abroad", "Withholding tax obligations", "Double taxation without treaty relief" },
                new() { "PE risk assessment matrix", "Remote working policy with tax approval gate", "Agent activity monitoring" },
                new() { "Treaty relief claims process", "Voluntary disclosure programme preparedness" },
                3, 4,
                new() { "GB", "DE", "US", "AU", "SG", "IE" },
                new() { "Technology", "Professional Services", "Manufacturing" },
                new() { "PE", "remote working", "BEPS Action 7" }),

            RiskBankEntry.Create(
                "VAT Recovery on Mixed-Use Costs",
                TaxType.IndirectTax,
                "Partial exemption calculations may under or over-recover input VAT on overhead costs shared between taxable and exempt activities.",
                new() { "Complex holding company structures", "Financial services activities", "Inaccurate attribution methodology" },
                new() { "Underpaid VAT and interest charges", "Over-recovery subject to claw-back", "Cash flow impact" },
                new() { "Annual partial exemption method review", "VAT returns sign-off by tax specialist" },
                new() { "Quarterly reconciliation", "HMRC ruling on methodology" },
                3, 3,
                new() { "GB", "DE", "FR" },
                new() { "Financial Services", "Real Estate", "Healthcare" },
                new() { "VAT", "partial exemption", "input tax" }),

            RiskBankEntry.Create(
                "Employment Tax — Disguised Remuneration",
                TaxType.EmploymentTax,
                "Benefits-in-kind or off-payroll arrangements may trigger disguised remuneration charges under IR35 or equivalent legislation.",
                new() { "Use of personal service companies", "EBT or trust structures", "Share plans without proper PAYE compliance" },
                new() { "PAYE and NIC arrears", "Class 1A NIC on BIK", "Regulatory penalties" },
                new() { "IR35 status determination process", "Off-payroll worker register", "P11D reconciliation controls" },
                new() { "Settlement with HMRC", "Voluntary restitution programme" },
                3, 4,
                new() { "GB" },
                new() { "Technology", "Professional Services", "Financial Services" },
                new() { "IR35", "off-payroll", "employment tax" }),

            RiskBankEntry.Create(
                "Customs Classification Errors",
                TaxType.Customs,
                "Goods imported using incorrect commodity codes lead to underpayment of customs duties and potential post-clearance demands.",
                new() { "Complex product portfolio", "Frequent product updates", "Lack of trained customs resource" },
                new() { "Post-clearance duty demands plus penalties", "Goods held at border", "Reputational damage with customs authority" },
                new() { "Commodity code classification policy", "Pre-shipment ruling applications", "Annual binding tariff information review" },
                new() { "Post-clearance audit protocol", "Amendment and refund procedures" },
                4, 3,
                new() { "GB", "DE", "FR", "NL", "US" },
                new() { "Manufacturing", "Retail", "Automotive" },
                new() { "customs", "tariff", "commodity codes" }),

            RiskBankEntry.Create(
                "CbCR Filing Obligations",
                TaxType.TransferPricing,
                "Country-by-Country Reporting obligations under BEPS Action 13 may be missed or incorrectly filed for large MNE groups.",
                new() { "Threshold monitoring failure", "Constituent entity changes", "System limitations in data aggregation" },
                new() { "Penalties for late or non-filing", "Tax authority risk assessment upgrades", "Regulatory scrutiny" },
                new() { "CbCR threshold monitoring control", "Filing calendar with statutory deadlines", "Data quality review process" },
                new() { "Corrective filing procedures", "Disclosure framework" },
                2, 5,
                new() { "GB", "DE", "FR", "US", "AU", "SG", "JP" },
                new() { "All" },
                new() { "CbCR", "BEPS", "country-by-country" }),

            RiskBankEntry.Create(
                "Withholding Tax on Cross-Border Payments",
                TaxType.CorporateIncomeTax,
                "Dividends, interest, and royalties paid to non-resident group entities may attract withholding tax where treaty relief is not properly claimed.",
                new() { "Treaty eligibility not assessed", "Beneficial ownership requirements not met", "Anti-avoidance provisions triggered" },
                new() { "Withholding tax cost becomes permanent", "Interest and penalties", "Restatement of financial statements" },
                new() { "Payment register with treaty review", "Beneficial ownership certification process", "WHT calendar" },
                new() { "Treaty relief claim procedures", "Tax authority ruling applications" },
                3, 4,
                new() { "GB", "NL", "LU", "IE", "US", "SG" },
                new() { "Financial Services", "Technology", "Manufacturing" },
                new() { "WHT", "treaty", "cross-border payments" }),

            RiskBankEntry.Create(
                "Payroll Tax — Expatriate Compliance",
                TaxType.Payroll,
                "Expatriate employees on international assignments may trigger payroll tax obligations in host countries that are not reflected in shadow payrolls.",
                new() { "Undocumented short-term business travellers", "Dual contract arrangements", "Social security coordination failures" },
                new() { "Payroll tax arrears in host jurisdiction", "Individual employee tax liability", "Social security underpayment" },
                new() { "Assignment letter review process", "Shadow payroll set-up controls", "Business traveller tracking system" },
                new() { "Voluntary disclosure to tax authority", "Individual tax equalisation adjustments" },
                3, 3,
                new() { "GB", "US", "DE", "FR", "AU", "SG" },
                new() { "All" },
                new() { "expatriate", "payroll", "shadow payroll" })
        };

        risksDb.RiskBankEntries.AddRange(riskBank);
        await risksDb.SaveChangesAsync();

        // ── Demo Client ───────────────────────────────────────────────────────
        if (clientsDb.Clients.Any()) return;

        var client = Client.Create("GlobalTech Industries plc", "Technology", 12);
        var entities = new[]
        {
            ("GlobalTech Industries plc (UK HoldCo)", "GB", "England & Wales", EntityType.GlobalParent, Region.EMEA),
            ("GlobalTech Netherlands BV", "NL", "Netherlands", EntityType.IntermediateHolding, Region.EMEA),
            ("GlobalTech GmbH", "DE", "Germany", EntityType.Subsidiary, Region.EMEA),
            ("GlobalTech Inc.", "US", "Delaware", EntityType.Subsidiary, Region.Americas),
            ("GlobalTech Asia Pte Ltd", "SG", "Singapore", EntityType.Subsidiary, Region.APAC),
            ("GlobalTech France SAS", "FR", "France", EntityType.Subsidiary, Region.EMEA),
        };

        foreach (var (name, country, jurisdiction, type, region) in entities)
            client.AddEntity(name, new CountryCode(country), jurisdiction, type, region);

        clientsDb.Clients.Add(client);
        await clientsDb.SaveChangesAsync();

        // ── RACM Entries ──────────────────────────────────────────────────────
        var clientId = new ClientId(client.Id.Value);

        var tpRisk = RacmEntry.CreateFromBank(clientId, riskBank[0], globalOwnerId, 4, 5);
        var peRisk = RacmEntry.CreateFromBank(clientId, riskBank[1], globalOwnerId, 3, 4);
        var vatRisk = RacmEntry.CreateFromBank(clientId, riskBank[2], globalOwnerId, 3, 3);
        var cbcrRisk = RacmEntry.CreateFromBank(clientId, riskBank[5], globalOwnerId, 2, 5);

        tpRisk.UpdateNetScore(3, 4);
        peRisk.UpdateNetScore(2, 4);
        vatRisk.UpdateNetScore(2, 3);
        cbcrRisk.UpdateNetScore(1, 5);

        var entityIds = client.Entities.Select(e => e.Id.Value).ToList();
        foreach (var eid in entityIds.Take(3))
            tpRisk.LinkEntity(new EntityId(eid));
        tpRisk.LinkEntity(new EntityId(entityIds[3]));

        risksDb.RacmEntries.AddRange(tpRisk, peRisk, vatRisk, cbcrRisk);
        await risksDb.SaveChangesAsync();

        // ── Controls ──────────────────────────────────────────────────────────
        var tpControl1 = Control.Create(
            tpRisk.Id, "Annual TP Documentation Review",
            "Prepare and review local file documentation for all intercompany transactions annually before filing deadline.",
            ControlType.Preventive, globalOwnerId, ControlFrequency.Annual);

        var tpControl2 = Control.Create(
            tpRisk.Id, "Intercompany Agreement Register",
            "Maintain a current register of all intercompany agreements with review dates.",
            ControlType.Preventive, globalOwnerId, ControlFrequency.Quarterly);

        var peControl = Control.Create(
            peRisk.Id, "Remote Worker PE Risk Assessment",
            "Assess permanent establishment exposure for all remote workers and business travellers exceeding 90 days.",
            ControlType.Detective, globalOwnerId, ControlFrequency.Quarterly);

        tpControl1.SubmitEvidence("TP_LocalFile_2024.pdf", "/evidence/tp-local-file-2024.pdf", globalOwnerId, "Annual local file signed off by Head of Tax.");
        tpControl2.SubmitEvidence("ICO_Register_Q4_2024.xlsx", "/evidence/ico-register-q4-2024.xlsx", globalOwnerId, "Q4 intercompany agreement register updated.");

        controlsDb.Controls.AddRange(tpControl1, tpControl2, peControl);
        await controlsDb.SaveChangesAsync();
    }
}
