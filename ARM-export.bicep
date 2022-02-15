weparam sites_helena_webapp_dev_name string = 'helena-webapp-dev'
param sites_helena_function_dev_name string = 'helena-function-dev'
param serverfarms_NorthEuropePlan_name string = 'NorthEuropePlan'
param serverfarms_helena_serviceplan_dev_name string = 'helena-serviceplan-dev'
param components_helena_appinsights_dev_name string = 'helena-appinsights-dev'
param components_helena_funinsights_dev_name string = 'helena-funinsights-dev'
param storageAccounts_helenafunctionstoragedev_name string = 'helenafunctionstoragedev'
param smartdetectoralertrules_failure_anomalies_helena_appinsights_dev_name string = 'failure anomalies - helena-appinsights-dev'
param smartdetectoralertrules_failure_anomalies_helena_funinsights_dev_name string = 'failure anomalies - helena-funinsights-dev'
param actiongroups_application_insights_smart_detection_externalid string = '/subscriptions/5bb1b2d9-ed37-4ee3-9053-56954eaa90c7/resourceGroups/keen-rg-dev/providers/microsoft.insights/actiongroups/application insights smart detection'

resource components_helena_appinsights_dev_name_resource 'microsoft.insights/components@2020-02-02' = {
  name: components_helena_appinsights_dev_name
  location: 'westeurope'
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Flow_Type: 'Redfield'
    Request_Source: 'WebTools16.0'
    RetentionInDays: 90
    IngestionMode: 'ApplicationInsights'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

resource components_helena_funinsights_dev_name_resource 'microsoft.insights/components@2020-02-02' = {
  name: components_helena_funinsights_dev_name
  location: 'northeurope'
  tags: {
    'hidden-link:/subscriptions/5bb1b2d9-ed37-4ee3-9053-56954eaa90c7/resourceGroups/helena-rg-dev/providers/Microsoft.Web/sites/helena-function-dev': 'Resource'
  }
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Flow_Type: 'Redfield'
    Request_Source: 'AppServiceEnablementCreate'
    RetentionInDays: 90
    IngestionMode: 'ApplicationInsights'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

resource storageAccounts_helenafunctionstoragedev_name_resource 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: storageAccounts_helenafunctionstoragedev_name
  location: 'northeurope'
  tags: {
    'hidden-related:/providers/Microsoft.Web/sites/helena-function-dev': 'empty'
  }
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
  kind: 'Storage'
  properties: {
    minimumTlsVersion: 'TLS1_0'
    allowBlobPublicAccess: true
    networkAcls: {
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: []
      defaultAction: 'Allow'
    }
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        file: {
          keyType: 'Account'
          enabled: true
        }
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
  }
}

resource serverfarms_helena_serviceplan_dev_name_resource 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: serverfarms_helena_serviceplan_dev_name
  location: 'West Europe'
  sku: {
    name: 'F1'
    tier: 'Free'
    size: 'F1'
    family: 'F'
    capacity: 0
  }
  kind: 'app'
  properties: {
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 1
    isSpot: false
    reserved: false
    isXenon: false
    hyperV: false
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
  }
}

resource serverfarms_NorthEuropePlan_name_resource 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: serverfarms_NorthEuropePlan_name
  location: 'North Europe'
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    family: 'Y'
    capacity: 0
  }
  kind: 'functionapp'
  properties: {
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 1
    isSpot: false
    reserved: false
    isXenon: false
    hyperV: false
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
  }
}

resource smartdetectoralertrules_failure_anomalies_helena_appinsights_dev_name_resource 'microsoft.alertsmanagement/smartdetectoralertrules@2021-04-01' = {
  name: smartdetectoralertrules_failure_anomalies_helena_appinsights_dev_name
  location: 'global'
  properties: {
    description: 'Failure Anomalies notifies you of an unusual rise in the rate of failed HTTP requests or dependency calls.'
    state: 'Enabled'
    severity: 'Sev3'
    frequency: 'PT1M'
    detector: {
      id: 'FailureAnomaliesDetector'
    }
    scope: [
      components_helena_appinsights_dev_name_resource.id
    ]
    actionGroups: {
      groupIds: [
        actiongroups_application_insights_smart_detection_externalid
      ]
    }
  }
}

resource smartdetectoralertrules_failure_anomalies_helena_funinsights_dev_name_resource 'microsoft.alertsmanagement/smartdetectoralertrules@2021-04-01' = {
  name: smartdetectoralertrules_failure_anomalies_helena_funinsights_dev_name
  location: 'global'
  properties: {
    description: 'Failure Anomalies notifies you of an unusual rise in the rate of failed HTTP requests or dependency calls.'
    state: 'Enabled'
    severity: 'Sev3'
    frequency: 'PT1M'
    detector: {
      id: 'FailureAnomaliesDetector'
    }
    scope: [
      components_helena_funinsights_dev_name_resource.id
    ]
    actionGroups: {
      groupIds: [
        actiongroups_application_insights_smart_detection_externalid
      ]
    }
  }
}

resource components_helena_appinsights_dev_name_degradationindependencyduration 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'degradationindependencyduration'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'degradationindependencyduration'
      DisplayName: 'Degradation in dependency duration'
      Description: 'Smart Detection rules notify you of performance anomaly issues.'
      HelpUrl: 'https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: false
      SupportsEmailNotifications: true
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_degradationindependencyduration 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'degradationindependencyduration'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'degradationindependencyduration'
      DisplayName: 'Degradation in dependency duration'
      Description: 'Smart Detection rules notify you of performance anomaly issues.'
      HelpUrl: 'https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: false
      SupportsEmailNotifications: true
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_appinsights_dev_name_degradationinserverresponsetime 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'degradationinserverresponsetime'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'degradationinserverresponsetime'
      DisplayName: 'Degradation in server response time'
      Description: 'Smart Detection rules notify you of performance anomaly issues.'
      HelpUrl: 'https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: false
      SupportsEmailNotifications: true
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_degradationinserverresponsetime 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'degradationinserverresponsetime'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'degradationinserverresponsetime'
      DisplayName: 'Degradation in server response time'
      Description: 'Smart Detection rules notify you of performance anomaly issues.'
      HelpUrl: 'https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: false
      SupportsEmailNotifications: true
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_appinsights_dev_name_digestMailConfiguration 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'digestMailConfiguration'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'digestMailConfiguration'
      DisplayName: 'Digest Mail Configuration'
      Description: 'This rule describes the digest mail preferences'
      HelpUrl: 'www.homail.com'
      IsHidden: true
      IsEnabledByDefault: true
      IsInPreview: false
      SupportsEmailNotifications: true
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_digestMailConfiguration 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'digestMailConfiguration'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'digestMailConfiguration'
      DisplayName: 'Digest Mail Configuration'
      Description: 'This rule describes the digest mail preferences'
      HelpUrl: 'www.homail.com'
      IsHidden: true
      IsEnabledByDefault: true
      IsInPreview: false
      SupportsEmailNotifications: true
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_appinsights_dev_name_extension_billingdatavolumedailyspikeextension 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'extension_billingdatavolumedailyspikeextension'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'extension_billingdatavolumedailyspikeextension'
      DisplayName: 'Abnormal rise in daily data volume (preview)'
      Description: 'This detection rule automatically analyzes the billing data generated by your application, and can warn you about an unusual increase in your application\'s billing costs'
      HelpUrl: 'https://github.com/Microsoft/ApplicationInsights-Home/tree/master/SmartDetection/billing-data-volume-daily-spike.md'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_extension_billingdatavolumedailyspikeextension 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'extension_billingdatavolumedailyspikeextension'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'extension_billingdatavolumedailyspikeextension'
      DisplayName: 'Abnormal rise in daily data volume (preview)'
      Description: 'This detection rule automatically analyzes the billing data generated by your application, and can warn you about an unusual increase in your application\'s billing costs'
      HelpUrl: 'https://github.com/Microsoft/ApplicationInsights-Home/tree/master/SmartDetection/billing-data-volume-daily-spike.md'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_appinsights_dev_name_extension_canaryextension 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'extension_canaryextension'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'extension_canaryextension'
      DisplayName: 'Canary extension'
      Description: 'Canary extension'
      HelpUrl: 'https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SmartDetection/'
      IsHidden: true
      IsEnabledByDefault: true
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_extension_canaryextension 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'extension_canaryextension'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'extension_canaryextension'
      DisplayName: 'Canary extension'
      Description: 'Canary extension'
      HelpUrl: 'https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SmartDetection/'
      IsHidden: true
      IsEnabledByDefault: true
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_appinsights_dev_name_extension_exceptionchangeextension 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'extension_exceptionchangeextension'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'extension_exceptionchangeextension'
      DisplayName: 'Abnormal rise in exception volume (preview)'
      Description: 'This detection rule automatically analyzes the exceptions thrown in your application, and can warn you about unusual patterns in your exception telemetry.'
      HelpUrl: 'https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SmartDetection/abnormal-rise-in-exception-volume.md'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_extension_exceptionchangeextension 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'extension_exceptionchangeextension'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'extension_exceptionchangeextension'
      DisplayName: 'Abnormal rise in exception volume (preview)'
      Description: 'This detection rule automatically analyzes the exceptions thrown in your application, and can warn you about unusual patterns in your exception telemetry.'
      HelpUrl: 'https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SmartDetection/abnormal-rise-in-exception-volume.md'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_appinsights_dev_name_extension_memoryleakextension 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'extension_memoryleakextension'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'extension_memoryleakextension'
      DisplayName: 'Potential memory leak detected (preview)'
      Description: 'This detection rule automatically analyzes the memory consumption of each process in your application, and can warn you about potential memory leaks or increased memory consumption.'
      HelpUrl: 'https://github.com/Microsoft/ApplicationInsights-Home/tree/master/SmartDetection/memory-leak.md'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_extension_memoryleakextension 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'extension_memoryleakextension'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'extension_memoryleakextension'
      DisplayName: 'Potential memory leak detected (preview)'
      Description: 'This detection rule automatically analyzes the memory consumption of each process in your application, and can warn you about potential memory leaks or increased memory consumption.'
      HelpUrl: 'https://github.com/Microsoft/ApplicationInsights-Home/tree/master/SmartDetection/memory-leak.md'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_appinsights_dev_name_extension_securityextensionspackage 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'extension_securityextensionspackage'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'extension_securityextensionspackage'
      DisplayName: 'Potential security issue detected (preview)'
      Description: 'This detection rule automatically analyzes the telemetry generated by your application and detects potential security issues.'
      HelpUrl: 'https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SmartDetection/application-security-detection-pack.md'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_extension_securityextensionspackage 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'extension_securityextensionspackage'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'extension_securityextensionspackage'
      DisplayName: 'Potential security issue detected (preview)'
      Description: 'This detection rule automatically analyzes the telemetry generated by your application and detects potential security issues.'
      HelpUrl: 'https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SmartDetection/application-security-detection-pack.md'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_appinsights_dev_name_extension_traceseveritydetector 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'extension_traceseveritydetector'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'extension_traceseveritydetector'
      DisplayName: 'Degradation in trace severity ratio (preview)'
      Description: 'This detection rule automatically analyzes the trace logs emitted from your application, and can warn you about unusual patterns in the severity of your trace telemetry.'
      HelpUrl: 'https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SmartDetection/degradation-in-trace-severity-ratio.md'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_extension_traceseveritydetector 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'extension_traceseveritydetector'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'extension_traceseveritydetector'
      DisplayName: 'Degradation in trace severity ratio (preview)'
      Description: 'This detection rule automatically analyzes the trace logs emitted from your application, and can warn you about unusual patterns in the severity of your trace telemetry.'
      HelpUrl: 'https://github.com/Microsoft/ApplicationInsights-Home/blob/master/SmartDetection/degradation-in-trace-severity-ratio.md'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_appinsights_dev_name_longdependencyduration 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'longdependencyduration'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'longdependencyduration'
      DisplayName: 'Long dependency duration'
      Description: 'Smart Detection rules notify you of performance anomaly issues.'
      HelpUrl: 'https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: false
      SupportsEmailNotifications: true
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_longdependencyduration 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'longdependencyduration'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'longdependencyduration'
      DisplayName: 'Long dependency duration'
      Description: 'Smart Detection rules notify you of performance anomaly issues.'
      HelpUrl: 'https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: false
      SupportsEmailNotifications: true
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_appinsights_dev_name_migrationToAlertRulesCompleted 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'migrationToAlertRulesCompleted'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'migrationToAlertRulesCompleted'
      DisplayName: 'Migration To Alert Rules Completed'
      Description: 'A configuration that controls the migration state of Smart Detection to Smart Alerts'
      HelpUrl: 'https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics'
      IsHidden: true
      IsEnabledByDefault: false
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: false
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_migrationToAlertRulesCompleted 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'migrationToAlertRulesCompleted'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'migrationToAlertRulesCompleted'
      DisplayName: 'Migration To Alert Rules Completed'
      Description: 'A configuration that controls the migration state of Smart Detection to Smart Alerts'
      HelpUrl: 'https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics'
      IsHidden: true
      IsEnabledByDefault: false
      IsInPreview: true
      SupportsEmailNotifications: false
    }
    Enabled: false
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_appinsights_dev_name_slowpageloadtime 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'slowpageloadtime'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'slowpageloadtime'
      DisplayName: 'Slow page load time'
      Description: 'Smart Detection rules notify you of performance anomaly issues.'
      HelpUrl: 'https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: false
      SupportsEmailNotifications: true
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_slowpageloadtime 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'slowpageloadtime'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'slowpageloadtime'
      DisplayName: 'Slow page load time'
      Description: 'Smart Detection rules notify you of performance anomaly issues.'
      HelpUrl: 'https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: false
      SupportsEmailNotifications: true
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_appinsights_dev_name_slowserverresponsetime 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_appinsights_dev_name_resource
  name: 'slowserverresponsetime'
  location: 'westeurope'
  properties: {
    RuleDefinitions: {
      Name: 'slowserverresponsetime'
      DisplayName: 'Slow server response time'
      Description: 'Smart Detection rules notify you of performance anomaly issues.'
      HelpUrl: 'https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: false
      SupportsEmailNotifications: true
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource components_helena_funinsights_dev_name_slowserverresponsetime 'microsoft.insights/components/ProactiveDetectionConfigs@2018-05-01-preview' = {
  parent: components_helena_funinsights_dev_name_resource
  name: 'slowserverresponsetime'
  location: 'northeurope'
  properties: {
    RuleDefinitions: {
      Name: 'slowserverresponsetime'
      DisplayName: 'Slow server response time'
      Description: 'Smart Detection rules notify you of performance anomaly issues.'
      HelpUrl: 'https://docs.microsoft.com/en-us/azure/application-insights/app-insights-proactive-performance-diagnostics'
      IsHidden: false
      IsEnabledByDefault: true
      IsInPreview: false
      SupportsEmailNotifications: true
    }
    Enabled: true
    SendEmailsToSubscriptionOwners: true
    CustomEmails: []
  }
}

resource storageAccounts_helenafunctionstoragedev_name_default 'Microsoft.Storage/storageAccounts/blobServices@2021-06-01' = {
  parent: storageAccounts_helenafunctionstoragedev_name_resource
  name: 'default'
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
  properties: {
    cors: {
      corsRules: []
    }
    deleteRetentionPolicy: {
      enabled: false
    }
  }
}

resource Microsoft_Storage_storageAccounts_fileServices_storageAccounts_helenafunctionstoragedev_name_default 'Microsoft.Storage/storageAccounts/fileServices@2021-06-01' = {
  parent: storageAccounts_helenafunctionstoragedev_name_resource
  name: 'default'
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
  properties: {
    protocolSettings: {
      smb: {}
    }
    cors: {
      corsRules: []
    }
    shareDeleteRetentionPolicy: {
      enabled: true
      days: 7
    }
  }
}

resource Microsoft_Storage_storageAccounts_queueServices_storageAccounts_helenafunctionstoragedev_name_default 'Microsoft.Storage/storageAccounts/queueServices@2021-06-01' = {
  parent: storageAccounts_helenafunctionstoragedev_name_resource
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
  }
}

resource Microsoft_Storage_storageAccounts_tableServices_storageAccounts_helenafunctionstoragedev_name_default 'Microsoft.Storage/storageAccounts/tableServices@2021-06-01' = {
  parent: storageAccounts_helenafunctionstoragedev_name_resource
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
  }
}

resource sites_helena_function_dev_name_resource 'Microsoft.Web/sites@2021-02-01' = {
  name: sites_helena_function_dev_name
  location: 'North Europe'
  kind: 'functionapp'
  properties: {
    enabled: true
    hostNameSslStates: [
      {
        name: '${sites_helena_function_dev_name}.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${sites_helena_function_dev_name}.scm.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]
    serverFarmId: serverfarms_NorthEuropePlan_name_resource.id
    reserved: false
    isXenon: false
    hyperV: false
    siteConfig: {
      numberOfWorkers: 1
      acrUseManagedIdentityCreds: false
      alwaysOn: false
      http20Enabled: false
      functionAppScaleLimit: 200
      minimumElasticInstanceCount: 1
    }
    scmSiteAlsoStopped: false
    clientAffinityEnabled: false
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    customDomainVerificationId: '7D479BC99F59226819EA18928C9D13FCC1356079EA58351571A6F8466440FA85'
    containerSize: 1536
    dailyMemoryTimeQuota: 0
    httpsOnly: true
    redundancyMode: 'None'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
  }
}

resource sites_helena_webapp_dev_name_resource 'Microsoft.Web/sites@2021-02-01' = {
  name: sites_helena_webapp_dev_name
  location: 'West Europe'
  kind: 'app'
  properties: {
    enabled: true
    hostNameSslStates: [
      {
        name: '${sites_helena_webapp_dev_name}.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${sites_helena_webapp_dev_name}.scm.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]
    serverFarmId: serverfarms_helena_serviceplan_dev_name_resource.id
    reserved: false
    isXenon: false
    hyperV: false
    siteConfig: {
      numberOfWorkers: 1
      acrUseManagedIdentityCreds: false
      alwaysOn: false
      http20Enabled: false
      functionAppScaleLimit: 0
      minimumElasticInstanceCount: 0
    }
    scmSiteAlsoStopped: false
    clientAffinityEnabled: true
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    customDomainVerificationId: '7D479BC99F59226819EA18928C9D13FCC1356079EA58351571A6F8466440FA85'
    containerSize: 0
    dailyMemoryTimeQuota: 0
    httpsOnly: false
    redundancyMode: 'None'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
  }
}

resource sites_helena_function_dev_name_ftp 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: 'ftp'
  location: 'North Europe'
  properties: {
    allow: true
  }
}

resource sites_helena_webapp_dev_name_ftp 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2021-02-01' = {
  parent: sites_helena_webapp_dev_name_resource
  name: 'ftp'
  location: 'West Europe'
  properties: {
    allow: true
  }
}

resource sites_helena_function_dev_name_scm 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: 'scm'
  location: 'North Europe'
  properties: {
    allow: true
  }
}

resource sites_helena_webapp_dev_name_scm 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2021-02-01' = {
  parent: sites_helena_webapp_dev_name_resource
  name: 'scm'
  location: 'West Europe'
  properties: {
    allow: true
  }
}

resource sites_helena_function_dev_name_web 'Microsoft.Web/sites/config@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: 'web'
  location: 'North Europe'
  properties: {
    numberOfWorkers: 1
    defaultDocuments: [
      'Default.htm'
      'Default.html'
      'Default.asp'
      'index.htm'
      'index.html'
      'iisstart.htm'
      'default.aspx'
      'index.php'
    ]
    netFrameworkVersion: 'v6.0'
    phpVersion: '5.6'
    requestTracingEnabled: false
    remoteDebuggingEnabled: false
    remoteDebuggingVersion: 'VS2019'
    httpLoggingEnabled: false
    acrUseManagedIdentityCreds: false
    logsDirectorySizeLimit: 35
    detailedErrorLoggingEnabled: false
    publishingUsername: '$helena-function-dev'
    scmType: 'None'
    use32BitWorkerProcess: true
    webSocketsEnabled: false
    alwaysOn: false
    managedPipelineMode: 'Integrated'
    virtualApplications: [
      {
        virtualPath: '/'
        physicalPath: 'site\\wwwroot'
        preloadEnabled: false
      }
    ]
    loadBalancing: 'LeastRequests'
    experiments: {
      rampUpRules: []
    }
    autoHealEnabled: false
    vnetRouteAllEnabled: false
    vnetPrivatePortsCount: 0
    localMySqlEnabled: false
    ipSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 1
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 1
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictionsUseMain: false
    http20Enabled: false
    minTlsVersion: '1.2'
    scmMinTlsVersion: '1.0'
    ftpsState: 'FtpsOnly'
    preWarmedInstanceCount: 0
    functionAppScaleLimit: 200
    functionsRuntimeScaleMonitoringEnabled: false
    minimumElasticInstanceCount: 1
    azureStorageAccounts: {}
  }
}

resource sites_helena_webapp_dev_name_web 'Microsoft.Web/sites/config@2021-02-01' = {
  parent: sites_helena_webapp_dev_name_resource
  name: 'web'
  location: 'West Europe'
  properties: {
    numberOfWorkers: 1
    defaultDocuments: [
      'Default.htm'
      'Default.html'
      'Default.asp'
      'index.htm'
      'index.html'
      'iisstart.htm'
      'default.aspx'
      'index.php'
      'hostingstart.html'
    ]
    netFrameworkVersion: 'v6.0'
    requestTracingEnabled: false
    remoteDebuggingEnabled: false
    httpLoggingEnabled: false
    acrUseManagedIdentityCreds: false
    logsDirectorySizeLimit: 35
    detailedErrorLoggingEnabled: false
    publishingUsername: '$helena-webapp-dev'
    scmType: 'None'
    use32BitWorkerProcess: true
    webSocketsEnabled: false
    alwaysOn: false
    managedPipelineMode: 'Integrated'
    virtualApplications: [
      {
        virtualPath: '/'
        physicalPath: 'site\\wwwroot'
        preloadEnabled: false
      }
    ]
    loadBalancing: 'LeastRequests'
    experiments: {
      rampUpRules: []
    }
    autoHealEnabled: false
    vnetRouteAllEnabled: false
    vnetPrivatePortsCount: 0
    localMySqlEnabled: false
    ipSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 1
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 1
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictionsUseMain: false
    http20Enabled: false
    minTlsVersion: '1.2'
    scmMinTlsVersion: '1.0'
    ftpsState: 'AllAllowed'
    preWarmedInstanceCount: 0
    functionAppScaleLimit: 0
    functionsRuntimeScaleMonitoringEnabled: false
    minimumElasticInstanceCount: 0
    azureStorageAccounts: {}
  }
}

resource sites_helena_function_dev_name_02d144aed1024828a46a2771cc955f6a 'Microsoft.Web/sites/deployments@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: '02d144aed1024828a46a2771cc955f6a'
  location: 'North Europe'
  properties: {
    status: 4
    author_email: 'N/A'
    author: 'N/A'
    deployer: 'ZipDeploy'
    message: 'Created via a push deployment'
    start_time: '2022-02-09T13:04:46.509471Z'
    end_time: '2022-02-09T13:04:48.0971203Z'
    active: false
  }
}

resource sites_helena_function_dev_name_099596dbf7e149c2b8631a26000bda00 'Microsoft.Web/sites/deployments@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: '099596dbf7e149c2b8631a26000bda00'
  location: 'North Europe'
  properties: {
    status: 4
    author_email: 'N/A'
    author: 'N/A'
    deployer: 'ZipDeploy'
    message: 'Created via a push deployment'
    start_time: '2022-02-09T14:37:54.1669776Z'
    end_time: '2022-02-09T14:37:56.1357474Z'
    active: true
  }
}

resource sites_helena_function_dev_name_2092b2caa466450c93743afc255a038b 'Microsoft.Web/sites/deployments@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: '2092b2caa466450c93743afc255a038b'
  location: 'North Europe'
  properties: {
    status: 4
    author_email: 'N/A'
    author: 'N/A'
    deployer: 'ZipDeploy'
    message: 'Created via a push deployment'
    start_time: '2022-02-09T13:01:09.2148577Z'
    end_time: '2022-02-09T13:01:10.9179833Z'
    active: false
  }
}

resource sites_helena_function_dev_name_Coffee 'Microsoft.Web/sites/functions@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: 'Coffee'
  location: 'North Europe'
  properties: {
    script_root_path_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/Coffee/'
    script_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/bin/GreetingService.API.Function.dll'
    config_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/Coffee/function.json'
    test_data_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/data/Functions/sampledata/Coffee.dat'
    href: 'https://helena-function-dev.azurewebsites.net/admin/functions/Coffee'
    config: {}
    invoke_url_template: 'https://helena-function-dev.azurewebsites.net/api/coffee'
    language: 'DotNetAssembly'
    isDisabled: false
  }
}

resource sites_helena_function_dev_name_DeleteGreeting 'Microsoft.Web/sites/functions@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: 'DeleteGreeting'
  location: 'North Europe'
  properties: {
    script_root_path_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/DeleteGreeting/'
    script_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/bin/GreetingService.API.Function.dll'
    config_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/DeleteGreeting/function.json'
    test_data_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/data/Functions/sampledata/DeleteGreeting.dat'
    href: 'https://helena-function-dev.azurewebsites.net/admin/functions/DeleteGreeting'
    config: {}
    invoke_url_template: 'https://helena-function-dev.azurewebsites.net/api/greeting/{id}'
    language: 'DotNetAssembly'
    isDisabled: false
  }
}

resource sites_helena_function_dev_name_GetGreeting 'Microsoft.Web/sites/functions@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: 'GetGreeting'
  location: 'North Europe'
  properties: {
    script_root_path_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/GetGreeting/'
    script_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/bin/GreetingService.API.Function.dll'
    config_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/GetGreeting/function.json'
    test_data_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/data/Functions/sampledata/GetGreeting.dat'
    href: 'https://helena-function-dev.azurewebsites.net/admin/functions/GetGreeting'
    config: {}
    invoke_url_template: 'https://helena-function-dev.azurewebsites.net/api/greeting/{id}'
    language: 'DotNetAssembly'
    isDisabled: false
  }
}

resource sites_helena_function_dev_name_GetGreetings 'Microsoft.Web/sites/functions@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: 'GetGreetings'
  location: 'North Europe'
  properties: {
    script_root_path_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/GetGreetings/'
    script_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/bin/GreetingService.API.Function.dll'
    config_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/GetGreetings/function.json'
    test_data_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/data/Functions/sampledata/GetGreetings.dat'
    href: 'https://helena-function-dev.azurewebsites.net/admin/functions/GetGreetings'
    config: {}
    invoke_url_template: 'https://helena-function-dev.azurewebsites.net/api/greeting'
    language: 'DotNetAssembly'
    isDisabled: false
  }
}

resource sites_helena_function_dev_name_PostGreeting 'Microsoft.Web/sites/functions@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: 'PostGreeting'
  location: 'North Europe'
  properties: {
    script_root_path_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/PostGreeting/'
    script_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/bin/GreetingService.API.Function.dll'
    config_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/PostGreeting/function.json'
    test_data_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/data/Functions/sampledata/PostGreeting.dat'
    href: 'https://helena-function-dev.azurewebsites.net/admin/functions/PostGreeting'
    config: {}
    invoke_url_template: 'https://helena-function-dev.azurewebsites.net/api/greeting'
    language: 'DotNetAssembly'
    isDisabled: false
  }
}

resource sites_helena_function_dev_name_PutGreeting 'Microsoft.Web/sites/functions@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: 'PutGreeting'
  location: 'North Europe'
  properties: {
    script_root_path_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/PutGreeting/'
    script_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/bin/GreetingService.API.Function.dll'
    config_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/site/wwwroot/PutGreeting/function.json'
    test_data_href: 'https://helena-function-dev.azurewebsites.net/admin/vfs/data/Functions/sampledata/PutGreeting.dat'
    href: 'https://helena-function-dev.azurewebsites.net/admin/functions/PutGreeting'
    config: {}
    invoke_url_template: 'https://helena-function-dev.azurewebsites.net/api/greeting'
    language: 'DotNetAssembly'
    isDisabled: false
  }
}

resource sites_helena_function_dev_name_sites_helena_function_dev_name_azurewebsites_net 'Microsoft.Web/sites/hostNameBindings@2021-02-01' = {
  parent: sites_helena_function_dev_name_resource
  name: '${sites_helena_function_dev_name}.azurewebsites.net'
  location: 'North Europe'
  properties: {
    siteName: 'helena-function-dev'
    hostNameType: 'Verified'
  }
}

resource sites_helena_webapp_dev_name_sites_helena_webapp_dev_name_azurewebsites_net 'Microsoft.Web/sites/hostNameBindings@2021-02-01' = {
  parent: sites_helena_webapp_dev_name_resource
  name: '${sites_helena_webapp_dev_name}.azurewebsites.net'
  location: 'West Europe'
  properties: {
    siteName: 'helena-webapp-dev'
    hostNameType: 'Verified'
  }
}

resource sites_helena_webapp_dev_name_Microsoft_AspNetCore_AzureAppServices_SiteExtension 'Microsoft.Web/sites/siteextensions@2021-02-01' = {
  parent: sites_helena_webapp_dev_name_resource
  name: 'Microsoft.AspNetCore.AzureAppServices.SiteExtension'
  location: 'West Europe'
}

resource storageAccounts_helenafunctionstoragedev_name_default_azure_webjobs_hosts 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-06-01' = {
  parent: storageAccounts_helenafunctionstoragedev_name_default
  name: 'azure-webjobs-hosts'
  properties: {
    immutableStorageWithVersioning: {
      enabled: false
    }
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
  dependsOn: [
    storageAccounts_helenafunctionstoragedev_name_resource
  ]
}

resource storageAccounts_helenafunctionstoragedev_name_default_azure_webjobs_secrets 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-06-01' = {
  parent: storageAccounts_helenafunctionstoragedev_name_default
  name: 'azure-webjobs-secrets'
  properties: {
    immutableStorageWithVersioning: {
      enabled: false
    }
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
  dependsOn: [
    storageAccounts_helenafunctionstoragedev_name_resource
  ]
}

resource storageAccounts_helenafunctionstoragedev_name_default_helena_function_dev 'Microsoft.Storage/storageAccounts/fileServices/shares@2021-06-01' = {
  parent: Microsoft_Storage_storageAccounts_fileServices_storageAccounts_helenafunctionstoragedev_name_default
  name: 'helena-function-dev'
  properties: {
    accessTier: 'TransactionOptimized'
    shareQuota: 5120
    enabledProtocols: 'SMB'
  }
  dependsOn: [
    storageAccounts_helenafunctionstoragedev_name_resource
  ]
}
