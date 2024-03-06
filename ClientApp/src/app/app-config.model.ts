export interface IAppConfig {
    environment: {
        name: string;
        production: boolean;
    };
    appInsights: {
        instrumentationKey: string;
    };
    logging: {
        console: boolean;
        appInsights: boolean;
    };
    api: {
        url: string;
    };
    buildInfo: {
        number: string;
        createdOn: string;
    };
    google: {
        clientId: string;
    };
    github: {
        clientId: string;
    };
    environments: IEnvironment[];
}

export interface IEnvironment {
    name: string,
    apiUrl: string,
    allow: boolean;
}
