import React, { useEffect, useState } from "react";
import { CompanyCompData } from "../../company";
import { getCompData } from "../../api";
import CompFinderItem from "./CompFinderItem/CompFinderItem";

type Props = {
  ticker: string;
};

const CompFinder = ({ ticker }: Props) => {
  const [companyDatas, setCompany] = useState<CompanyCompData[]>();
  useEffect(() => {
    const getComps = async () => {
      const value = await getCompData(ticker);
      setCompany(value?.data);
    };
    getComps();
  }, [ticker]);
  return (
    <div className="inline-flex rounded-md shadow-sm m-4">
      {companyDatas?.map((companyData) => {
        return <CompFinderItem ticker={companyData.symbol} />;
      })}
    </div>
  );
};

export default CompFinder;
