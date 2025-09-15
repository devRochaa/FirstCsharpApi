import React, { SyntheticEvent, useEffect, useState } from "react";
import { CompanySearch } from "../../company";
import { searchCompanies } from "../../api";
import ListPortfolio from "../../Components/Portifolio/ListPortfolio/ListPortfolio";
import CardList from "../../Components/CardList/CardList";
import Search from "../../Components/Search/Search";
import Navbar from "../../Components/Navbar/Navbar";
import { PortifolioGet } from "../../Models/Portifolio";
import {
  portifolioDeleteAPI,
  portifolioGetAPI,
  portifolioPostAPI,
} from "../../Services/PortifolioService";
import { toast } from "react-toastify";

interface Props {}

const SearchPage = (props: Props) => {
  const [search, setSearch] = useState<string>("");
  const [searchResult, setSearchResult] = useState<CompanySearch[]>([]);
  const [serverError, setServerError] = useState<string>("");
  const [portfolioValues, setPortifolioValues] = useState<
    PortifolioGet[] | null
  >([]);

  useEffect(() => {
    getPortifolio();
  }, []);

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value);
  };

  const getPortifolio = () => {
    portifolioGetAPI()
      .then((res) => {
        if (res?.data) {
          setPortifolioValues(res?.data);
        }
      })
      .catch((e) => {
        toast.warning("Could not get portfolio values.");
        setPortifolioValues(null);
      });
  };

  const onPortfolioCreate = (e: any) => {
    e.preventDefault();
    portifolioPostAPI(e.target[0].value)
      .then((res) => {
        if (res?.status === 201) {
          getPortifolio();
          toast.success("Stock added to portfolio!");
        }
      })
      .catch((e) => {
        toast.warning("Could not add portfolio.");
      });
  };

  const onPortfolioRemove = (e: any) => {
    e.preventDefault();
    portifolioDeleteAPI(e.target[0].value)
      .then((res) => {
        if (res?.status == 200) {
          getPortifolio();
          toast.success("Stock removed!");
        }
      })
      .catch((e) => {
        toast.warning("Could not be removed from your portfolio.");
      });
  };

  const onSearchSubmit = async (e: SyntheticEvent) => {
    e.preventDefault();
    const result = await searchCompanies(search);
    if (typeof result === "string") {
      setServerError(result);
      console.log(serverError);
    } else if (Array.isArray(result.data)) {
      setSearchResult(result.data);
    }
    //console.log(searchResult);
  };

  return (
    <div className="App">
      <Search
        onSearchSubmit={onSearchSubmit}
        search={search}
        handleSearchChange={handleSearchChange}
      />
      <ListPortfolio
        portfolioValues={portfolioValues!}
        onPortfolioRemove={onPortfolioRemove}
      />
      <CardList
        searchResults={searchResult}
        onPortfolioCreate={onPortfolioCreate}
      />
      {serverError && <h1>{serverError}</h1>}
    </div>
  );
};

export default SearchPage;
