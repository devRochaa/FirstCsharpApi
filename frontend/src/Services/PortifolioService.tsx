import axios from "axios";
import { handleError } from "../Helpers/ErrorHandler";
import { PortifolioGet, PortifolioPost } from "../Models/Portifolio";

const api = process.env.REACT_APP_BACKEND_API_URL + "portifolio";

export const portifolioPostAPI = async (symbol: string) => {
  try {
    const data = await axios.post<PortifolioPost>(api + `/${symbol}`);
    return data;
  } catch (error) {
    handleError(error);
  }
};

export const portifolioDeleteAPI = async (symbol: string) => {
  try {
    const data = await axios.delete<PortifolioPost>(api + `?symbol=${symbol}`);
    return data;
  } catch (error) {
    handleError(error);
  }
};

export const portifolioGetAPI = async () => {
  try {
    const data = await axios.get<PortifolioGet[]>(api);
    return data;
  } catch (error) {
    handleError(error);
  }
};
