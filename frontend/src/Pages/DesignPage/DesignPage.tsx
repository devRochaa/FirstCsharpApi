import React from "react";
import Table from "../../Components/Table/Table";
import RatioList from "../../Components/RatioList/RatioList";
import "./DesignPage.css";
import { testIncomeStatementData } from "../../Components/Table/testData";
import { config } from "dotenv";

interface Props {}

const tableConfig = [
  {
    label: "Market Cap",
    render: (company: any) => company.marketCap,
    subTitle: "Total value of all a company's shares of stock",
  },
];

const DesignPage = (props: Props) => {
  return (
    <div className="DesignPage">
      <h1>FinShark Design Page</h1>
      <h2>This is where we well house various designs aspects of the app</h2>
      <RatioList data={testIncomeStatementData} config={tableConfig} />
      <Table data={testIncomeStatementData} config={tableConfig} />
    </div>
  );
};

export default DesignPage;
