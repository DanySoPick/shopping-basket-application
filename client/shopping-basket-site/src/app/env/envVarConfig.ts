export interface IEnvVarConfig {
    environment: string;
    apiUrl: string;
  }

  var defaultConfig: IEnvVarConfig = {
    environment: "local",
    apiUrl: "https://localhost:7236",
  };

  export default <IEnvVarConfig>{ ...defaultConfig };
  