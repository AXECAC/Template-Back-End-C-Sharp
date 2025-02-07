--
-- PostgreSQL database dump
--

-- Dumped from database version 17.2 (Debian 17.2-1.pgdg120+1)
-- Dumped by pg_dump version 17.2 (Debian 17.2-1.pgdg120+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Users; Type: TABLE; Schema: public; Owner: aragami
--

CREATE TABLE public."Users" (
    "Id" integer NOT NULL,
    "FirstName" text NOT NULL,
    "SecondName" text NOT NULL,
    "Email" text NOT NULL,
    "Password" text NOT NULL
);


ALTER TABLE public."Users" OWNER TO aragami;

--
-- Name: Users_Id_seq; Type: SEQUENCE; Schema: public; Owner: aragami
--

ALTER TABLE public."Users" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Users_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: aragami
--

COPY public."Users" ("Id", "FirstName", "SecondName", "Email", "Password") FROM stdin;
\.


--
-- Name: Users_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: aragami
--

SELECT pg_catalog.setval('public."Users_Id_seq"', 1, false);


--
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: aragami
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- PostgreSQL database dump complete
--

